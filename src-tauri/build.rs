fn main() {
    println!("cargo:rerun-if-changed=../src/wwwroot");
    tauri_build::build()
}
