use base64::Engine;
use ed25519_dalek::{Signature, VerifyingKey};
use serde::{Deserialize, Serialize};
use sha2::{Digest, Sha256};
use std::collections::{BTreeMap, HashMap};
use std::fs::{self, File};
use std::io::{BufReader, Read};
use std::path::{Path, PathBuf};
use std::sync::{Mutex, OnceLock};
use std::time::UNIX_EPOCH;
use zip::ZipArchive;

/// Embedded Ed25519 public key for official .server packages signed by MakeServer.
pub const OFFICIAL_PUBLIC_KEY_B64: &str = "AACvdIPwAqVOYdoFngk32m2OiVtFwrZ2zMGz0H4Hpk0=";

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct PackageTrustDto {
    pub is_official: bool,
    pub status: String,
    pub reason: String,
    pub package_sha256: String,
}

#[derive(Deserialize)]
struct ServerSignatureFile {
    #[allow(dead_code)]
    version: u32,
    algorithm: String,
    #[serde(rename = "publicKey")]
    public_key: String,
    #[serde(rename = "manifestSha256")]
    manifest_sha256: String,
    signature: String,
    files: BTreeMap<String, String>,
}

fn sha256_hex_reader<R: Read>(mut reader: R) -> Result<String, String> {
    let mut hasher = Sha256::new();
    let mut buf = [0u8; 65536];
    loop {
        let n = reader
            .read(&mut buf)
            .map_err(|e| format!("Failed reading stream for SHA-256: {}", e))?;
        if n == 0 {
            break;
        }
        hasher.update(&buf[..n]);
    }
    Ok(hasher
        .finalize()
        .iter()
        .map(|b| format!("{:02x}", b))
        .collect())
}

pub fn sha256_hex_file(path: &Path) -> Result<String, String> {
    let f = File::open(path)
        .map_err(|e| format!("Failed to open '{}' for hashing: {}", path.display(), e))?;
    sha256_hex_reader(BufReader::new(f))
}

fn build_canonical_manifest(files: &BTreeMap<String, String>) -> String {
    let mut out = String::new();
    for (path, hash) in files {
        out.push_str(path);
        out.push('\t');
        out.push_str(hash);
        out.push('\n');
    }
    out
}

type VerifyCacheKey = (PathBuf, u64, u128);

static VERIFY_CACHE: OnceLock<Mutex<HashMap<VerifyCacheKey, PackageTrustDto>>> = OnceLock::new();

fn file_cache_key(path: &Path) -> Option<VerifyCacheKey> {
    let meta = fs::metadata(path).ok()?;
    let modified_ns = meta
        .modified()
        .ok()
        .and_then(|t| t.duration_since(UNIX_EPOCH).ok())
        .map(|d| d.as_nanos())
        .unwrap_or(0);
    Some((path.to_path_buf(), meta.len(), modified_ns))
}

pub fn verify_server_package(archive_path: &Path) -> PackageTrustDto {
    if let Some(key) = file_cache_key(archive_path) {
        if let Ok(guard) = VERIFY_CACHE.get_or_init(|| Mutex::new(HashMap::new())).lock() {
            if let Some(cached) = guard.get(&key) {
                return cached.clone();
            }
        }
        let result = verify_server_package_with_key(archive_path, OFFICIAL_PUBLIC_KEY_B64);
        if let Ok(mut guard) = VERIFY_CACHE.get_or_init(|| Mutex::new(HashMap::new())).lock() {
            guard.insert(key, result.clone());
        }
        return result;
    }
    verify_server_package_with_key(archive_path, OFFICIAL_PUBLIC_KEY_B64)
}

pub fn verify_server_package_with_key(
    archive_path: &Path,
    expected_public_key_b64: &str,
) -> PackageTrustDto {
    match verify_inner(archive_path, expected_public_key_b64) {
        Ok(()) => PackageTrustDto {
            is_official: true,
            status: "official".to_string(),
            reason: "Verified official Ed25519 signature.".to_string(),
            package_sha256: String::new(),
        },
        Err((status, reason)) => PackageTrustDto {
            is_official: false,
            status: status.to_string(),
            reason,
            package_sha256: sha256_hex_file(archive_path).unwrap_or_default(),
        },
    }
}

fn verify_inner(
    archive_path: &Path,
    expected_public_key_b64: &str,
) -> Result<(), (&'static str, String)> {
    if !archive_path.is_file() {
        return Err((
            "missing",
            format!("Package file '{}' not found.", archive_path.display()),
        ));
    }

    let file = File::open(archive_path)
        .map_err(|e| ("invalid", format!("Failed to open package: {}", e)))?;
    let mut archive = ZipArchive::new(BufReader::new(file))
        .map_err(|e| ("invalid", format!("Invalid .server archive: {}", e)))?;

    let mut sig_idx = None;
    let mut actual_files: HashMap<String, String> = HashMap::new();

    for i in 0..archive.len() {
        let mut entry = archive
            .by_index(i)
            .map_err(|e| ("invalid", format!("Failed reading archive entry: {}", e)))?;
        if entry.is_dir() {
            continue;
        }
        let norm_name = entry.name().replace('\\', "/");
        if norm_name.eq_ignore_ascii_case("signature.json") {
            sig_idx = Some(i);
            continue;
        }
        let hash = sha256_hex_reader(&mut entry)
            .map_err(|e| ("invalid", format!("Failed hashing '{}': {}", norm_name, e)))?;
        actual_files.insert(norm_name, hash);
    }

    let Some(idx) = sig_idx else {
        return Err((
            "unsigned",
            "This .server package is unsigned (no signature.json).".to_string(),
        ));
    };

    let mut sig_entry = archive
        .by_index(idx)
        .map_err(|e| ("invalid", format!("Failed reading signature.json: {}", e)))?;
    let mut sig_raw = String::new();
    sig_entry
        .read_to_string(&mut sig_raw)
        .map_err(|e| ("invalid", format!("Failed reading signature.json: {}", e)))?;

    let sig_file: ServerSignatureFile = serde_json::from_str(&sig_raw)
        .map_err(|e| ("invalid", format!("Invalid signature.json format: {}", e)))?;

    if !sig_file.algorithm.eq_ignore_ascii_case("Ed25519") {
        return Err((
            "invalid",
            format!("Unsupported signature algorithm '{}'.", sig_file.algorithm),
        ));
    }

    if sig_file.public_key.trim() != expected_public_key_b64.trim() {
        return Err((
            "untrusted_key",
            "Package was signed with an unrecognized public key.".to_string(),
        ));
    }

    let pub_bytes = base64::engine::general_purpose::STANDARD
        .decode(expected_public_key_b64.trim())
        .map_err(|e| ("invalid", format!("Invalid embedded public key: {}", e)))?;
    let pub_arr: [u8; 32] = pub_bytes
        .try_into()
        .map_err(|_| ("invalid", "Ed25519 public key must be 32 bytes.".to_string()))?;
    let verifying_key = VerifyingKey::from_bytes(&pub_arr)
        .map_err(|e| ("invalid", format!("Invalid Ed25519 public key: {}", e)))?;

    let sig_bytes = base64::engine::general_purpose::STANDARD
        .decode(sig_file.signature.trim())
        .map_err(|e| ("invalid", format!("Invalid base64 signature: {}", e)))?;
    let sig_arr: [u8; 64] = sig_bytes
        .try_into()
        .map_err(|_| ("invalid", "Ed25519 signature must be 64 bytes.".to_string()))?;
    let signature = Signature::from_bytes(&sig_arr);

    let manifest = build_canonical_manifest(&sig_file.files);
    if !sig_file.manifest_sha256.trim().is_empty() {
        let computed_manifest_hash: String = Sha256::digest(manifest.as_bytes())
            .iter()
            .map(|b| format!("{:02x}", b))
            .collect();
        if !computed_manifest_hash.eq_ignore_ascii_case(sig_file.manifest_sha256.trim()) {
            return Err((
                "invalid",
                "Manifest SHA-256 mismatch in signature.json.".to_string(),
            ));
        }
    }

    verifying_key
        .verify_strict(manifest.as_bytes(), &signature)
        .map_err(|_| (
            "invalid_signature",
            "Ed25519 signature verification failed.".to_string(),
        ))?;

    if actual_files.len() != sig_file.files.len() {
        return Err((
            "tampered",
            "Package contains extra or missing files not covered by signature.json.".to_string(),
        ));
    }

    for (path, expected_hash) in &sig_file.files {
        let Some(actual_hash) = actual_files.get(path) else {
            return Err((
                "tampered",
                format!("Signed file '{}' is missing from the package.", path),
            ));
        };
        if !actual_hash.eq_ignore_ascii_case(expected_hash.trim()) {
            return Err((
                "tampered",
                format!("File '{}' was modified after signing.", path),
            ));
        }
    }

    Ok(())
}

#[cfg(test)]
mod tests {
    use super::{build_canonical_manifest, verify_server_package_with_key};
    use base64::Engine;
    use ed25519_dalek::{Signer, SigningKey};
    use serde_json::json;
    use sha2::{Digest, Sha256};
    use std::collections::BTreeMap;
    use std::fs::{self, File};
    use std::io::Write;
    use std::time::{SystemTime, UNIX_EPOCH};
    use zip::write::SimpleFileOptions;
    use zip::ZipWriter;

    fn make_temp_zip_path(name: &str) -> std::path::PathBuf {
        let nanos = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .unwrap()
            .as_nanos();
        std::env::temp_dir().join(format!("sct_sig_test_{}_{}.server", name, nanos))
    }

    fn sha256_hex_bytes(b: &[u8]) -> String {
        Sha256::digest(b).iter().map(|x| format!("{:02x}", x)).collect()
    }

    fn write_signed_zip(
        path: &std::path::Path,
        signing_key: &SigningKey,
        extra_unsigned: bool,
        tamper_dll: bool,
    ) -> String {
        let meta = br#"{"Name":"Test"}"#;
        let dll = b"fake-dll-bytes";

        let mut files = BTreeMap::new();
        files.insert("test/metadata.json".to_string(), sha256_hex_bytes(meta));
        files.insert("test/test_funcs.dll".to_string(), sha256_hex_bytes(dll));

        let manifest = build_canonical_manifest(&files);
        let manifest_sha = sha256_hex_bytes(manifest.as_bytes());
        let sig = signing_key.sign(manifest.as_bytes());
        let pub_b64 = base64::engine::general_purpose::STANDARD
            .encode(signing_key.verifying_key().to_bytes());
        let sig_b64 = base64::engine::general_purpose::STANDARD.encode(sig.to_bytes());

        let sig_json = json!({
            "version": 1,
            "algorithm": "Ed25519",
            "publicKey": pub_b64,
            "manifestSha256": manifest_sha,
            "signature": sig_b64,
            "files": files,
        });

        let file = File::create(path).unwrap();
        let mut zip = ZipWriter::new(file);
        let opts = SimpleFileOptions::default();

        zip.start_file("test/metadata.json", opts).unwrap();
        zip.write_all(meta).unwrap();

        zip.start_file("test/test_funcs.dll", opts).unwrap();
        zip.write_all(if tamper_dll { b"tampered-dll" } else { dll })
            .unwrap();

        if extra_unsigned {
            zip.start_file("test/evil.dll", opts).unwrap();
            zip.write_all(b"evil").unwrap();
        }

        zip.start_file("signature.json", opts).unwrap();
        zip.write_all(serde_json::to_string_pretty(&sig_json).unwrap().as_bytes())
            .unwrap();
        zip.finish().unwrap();

        pub_b64
    }

    #[test]
    fn valid_signed_package_is_official() {
        let zip_path = make_temp_zip_path("valid");
        let sk = SigningKey::from_bytes(&[7u8; 32]);
        let pub_b64 = write_signed_zip(&zip_path, &sk, false, false);

        let res = verify_server_package_with_key(&zip_path, &pub_b64);
        assert!(res.is_official, "Expected official: {:?}", res);
        assert_eq!(res.status, "official");

        let _ = fs::remove_file(&zip_path);
    }

    #[test]
    fn tampered_dll_and_extra_file_and_wrong_key_fail_verification() {
        let sk = SigningKey::from_bytes(&[9u8; 32]);

        let p_tampered = make_temp_zip_path("tampered");
        let pub_b64 = write_signed_zip(&p_tampered, &sk, false, true);
        let res_tampered = verify_server_package_with_key(&p_tampered, &pub_b64);
        assert!(!res_tampered.is_official);
        assert_eq!(res_tampered.status, "tampered");

        let p_extra = make_temp_zip_path("extra");
        write_signed_zip(&p_extra, &sk, true, false);
        let res_extra = verify_server_package_with_key(&p_extra, &pub_b64);
        assert!(!res_extra.is_official);
        assert_eq!(res_extra.status, "tampered");

        let p_wrong_key = make_temp_zip_path("wrongkey");
        write_signed_zip(&p_wrong_key, &sk, false, false);
        let other_sk = SigningKey::from_bytes(&[1u8; 32]);
        let other_pub = base64::engine::general_purpose::STANDARD
            .encode(other_sk.verifying_key().to_bytes());
        let res_wrong = verify_server_package_with_key(&p_wrong_key, &other_pub);
        assert!(!res_wrong.is_official);
        assert_eq!(res_wrong.status, "untrusted_key");

        let _ = fs::remove_file(&p_tampered);
        let _ = fs::remove_file(&p_extra);
        let _ = fs::remove_file(&p_wrong_key);
    }
}
