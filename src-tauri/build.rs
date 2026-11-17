use std::path::{Path, PathBuf};
use std::process::Command;

fn detect_dotnet_tfm() -> Option<String> {
    let out = Command::new("dotnet").arg("--list-sdks").output().ok()?;
    if !out.status.success() {
        return None;
    }
    let stdout = String::from_utf8_lossy(&out.stdout);
    let mut best_major = 0u32;
    for line in stdout.lines() {
        if let Some(major_str) = line.trim().split('.').next() {
            if let Ok(major) = major_str.parse::<u32>() {
                if major >= 8 && major > best_major {
                    best_major = major;
                }
            }
        }
    }
    if best_major >= 8 {
        Some(format!("net{}.0", best_major))
    } else {
        None
    }
}

fn build_funcs_runner() {
    let out_dir = PathBuf::from(std::env::var_os("OUT_DIR").expect("OUT_DIR not set"));
    let manifest_dir =
        PathBuf::from(std::env::var_os("CARGO_MANIFEST_DIR").expect("CARGO_MANIFEST_DIR not set"));
    let runner_src_dir = manifest_dir.join("funcs_runner");
    let _ = std::fs::remove_dir_all(runner_src_dir.join("obj"));
    let _ = std::fs::remove_dir_all(runner_src_dir.join("bin"));

    println!("cargo:rerun-if-changed=funcs_runner/SctFuncsRunner.csproj");
    println!("cargo:rerun-if-changed=funcs_runner/Program.cs");

    let target_exe_name = if cfg!(target_os = "windows") {
        "SctFuncsRunner.exe"
    } else {
        "SctFuncsRunner"
    };
    let dest_exe = out_dir.join(target_exe_name);
    let build_src_dir = out_dir.join("funcs_runner_src");
    let pub_dir = out_dir.join("funcs_runner_pub");
    let tfm = detect_dotnet_tfm().unwrap_or_else(|| "net8.0".to_string());
    let rid = if cfg!(target_os = "windows") {
        "win-x64"
    } else {
        "linux-x64"
    };

    std::fs::create_dir_all(&build_src_dir).expect("Failed to create funcs_runner_src");
    let csproj_content = format!(
        "<Project Sdk=\"Microsoft.NET.Sdk\">\n  <PropertyGroup>\n    <OutputType>Exe</OutputType>\n    <TargetFramework>{}</TargetFramework>\n    <ImplicitUsings>enable</ImplicitUsings>\n    <Nullable>enable</Nullable>\n  </PropertyGroup>\n</Project>\n",
        tfm
    );
    let csproj = build_src_dir.join("SctFuncsRunner.csproj");
    std::fs::write(&csproj, csproj_content).expect("Failed to write SctFuncsRunner.csproj");
    std::fs::copy(
        runner_src_dir.join("Program.cs"),
        build_src_dir.join("Program.cs"),
    )
    .expect("Failed to copy Program.cs");

    let status = Command::new("dotnet")
        .args([
            "publish",
            csproj.to_string_lossy().as_ref(),
            "-c",
            "Release",
            "-r",
            rid,
            "--self-contained",
            "true",
            "-p:PublishSingleFile=true",
            "-p:EnableCompressionInSingleFile=true",
            "-p:IncludeNativeLibrariesForSelfExtract=true",
            "--nologo",
            "-o",
            pub_dir.to_string_lossy().as_ref(),
        ])
        .status()
        .expect("Failed to execute `dotnet publish` for SctFuncsRunner");

    assert!(
        status.success(),
        "`dotnet publish` for SctFuncsRunner failed with status: {:?}",
        status.code()
    );

    let published_exe = pub_dir.join(target_exe_name);
    assert!(
        Path::new(&published_exe).is_file(),
        "Expected published runner at {}",
        published_exe.display()
    );
    std::fs::copy(&published_exe, &dest_exe).expect("Failed to copy SctFuncsRunner binary to OUT_DIR");
}

fn main() {
    println!("cargo:rerun-if-changed=../src/wwwroot");
    build_funcs_runner();
    tauri_build::build()
}
