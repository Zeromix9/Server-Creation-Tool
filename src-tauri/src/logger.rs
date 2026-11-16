use std::fs::{self, OpenOptions};
use std::io::Write;
use std::path::{Path, PathBuf};
use std::sync::{Mutex, OnceLock};

static LOG_LOCK: OnceLock<Mutex<()>> = OnceLock::new();

fn log_mutex() -> &'static Mutex<()> {
    LOG_LOCK.get_or_init(|| Mutex::new(()))
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub(crate) struct LocalDateTime {
    pub year: u16,
    pub month: u16,
    pub day: u16,
    pub hour: u16,
    pub minute: u16,
}

#[cfg(target_os = "windows")]
fn current_local_datetime() -> LocalDateTime {
    #[repr(C)]
    struct SystemTimeWin {
        w_year: u16,
        w_month: u16,
        w_day_of_week: u16,
        w_day: u16,
        w_hour: u16,
        w_minute: u16,
        w_second: u16,
        w_milliseconds: u16,
    }

    extern "system" {
        fn GetLocalTime(lp_system_time: *mut SystemTimeWin);
    }

    let mut st = SystemTimeWin {
        w_year: 1970,
        w_month: 1,
        w_day_of_week: 0,
        w_day: 1,
        w_hour: 0,
        w_minute: 0,
        w_second: 0,
        w_milliseconds: 0,
    };
    unsafe {
        GetLocalTime(&mut st);
    }
    LocalDateTime {
        year: st.w_year,
        month: st.w_month,
        day: st.w_day,
        hour: st.w_hour,
        minute: st.w_minute,
    }
}

#[cfg(not(target_os = "windows"))]
fn current_local_datetime() -> LocalDateTime {
    let secs = std::time::SystemTime::now()
        .duration_since(std::time::UNIX_EPOCH)
        .map(|d| d.as_secs())
        .unwrap_or(0);
    let days = (secs / 86400) as i64;
    let rem = secs % 86400;
    let hour = (rem / 3600) as u16;
    let minute = ((rem % 3600) / 60) as u16;

    let z = days + 719468;
    let era = if z >= 0 { z } else { z - 146096 } / 146097;
    let doe = z - era * 146097;
    let yoe = (doe - doe / 1460 + doe / 36524 - doe / 146096) / 365;
    let y = yoe + era * 400;
    let doy = doe - (365 * yoe + yoe / 4 - yoe / 100);
    let mp = (5 * doy + 2) / 153;
    let d = doy - (153 * mp + 2) / 5 + 1;
    let m = mp + if mp < 10 { 3 } else { -9 };
    let year = (y + if m <= 2 { 1 } else { 0 }) as u16;

    LocalDateTime {
        year,
        month: m as u16,
        day: d as u16,
        hour,
        minute,
    }
}

pub(crate) fn format_log_filename(dt: LocalDateTime) -> String {
    format!("{:02}-{:02}-{:04}-log.txt", dt.day, dt.month, dt.year)
}

pub(crate) fn format_log_line(dt: LocalDateTime, message: &str) -> String {
    let am_pm = if dt.hour >= 12 { "PM" } else { "AM" };
    let hour_12 = match dt.hour % 12 {
        0 => 12,
        h => h,
    };
    let clean = message.trim_end_matches(['\r', '\n']);
    format!("{:02}:{:02} {}>>> {}\r\n", hour_12, dt.minute, am_pm, clean)
}

pub fn get_logs_dir() -> Result<PathBuf, String> {
    let exe_path = std::env::current_exe()
        .map_err(|e| format!("Failed to locate executable: {}", e))?;
    let exe_dir = exe_path
        .parent()
        .ok_or_else(|| "Failed to locate executable directory".to_string())?;
    Ok(exe_dir.join("sct_tool_logs"))
}

pub fn append_to_dir(logs_dir: &Path, message: &str) {
    let trimmed = message.trim();
    if trimmed.is_empty() {
        return;
    }

    let dt = current_local_datetime();
    let file_path = logs_dir.join(format_log_filename(dt));
    let line = format_log_line(dt, trimmed);

    let _guard = log_mutex().lock();
    if fs::create_dir_all(logs_dir).is_err() {
        return;
    }
    if let Ok(mut file) = OpenOptions::new().create(true).append(true).open(&file_path) {
        let _ = file.write_all(line.as_bytes());
    }
}

pub fn append(message: impl AsRef<str>) {
    if let Ok(dir) = get_logs_dir() {
        append_to_dir(&dir, message.as_ref());
    }
}

#[cfg(test)]
mod tests {
    use super::{append_to_dir, format_log_filename, format_log_line, LocalDateTime};
    use std::fs;
    use std::time::{SystemTime, UNIX_EPOCH};

    #[test]
    fn formats_legacy_log_line_and_filename() {
        let midnight = LocalDateTime {
            year: 2026,
            month: 11,
            day: 17,
            hour: 0,
            minute: 5,
        };
        assert_eq!(format_log_filename(midnight), "17-11-2026-log.txt");
        assert_eq!(
            format_log_line(midnight, "Application started"),
            "12:05 AM>>> Application started\r\n"
        );

        let afternoon = LocalDateTime {
            year: 2026,
            month: 11,
            day: 17,
            hour: 13,
            minute: 42,
        };
        assert_eq!(
            format_log_line(afternoon, "Backup complete\n"),
            "01:42 PM>>> Backup complete\r\n"
        );
    }

    #[test]
    fn appends_log_lines_to_directory() {
        let nanos = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .unwrap()
            .as_nanos();
        let dir = std::env::temp_dir().join(format!("sct_log_test_{}", nanos));
        append_to_dir(&dir, "First entry");
        append_to_dir(&dir, "Second entry");

        let mut files = fs::read_dir(&dir)
            .unwrap()
            .flatten()
            .map(|e| e.path())
            .collect::<Vec<_>>();
        assert_eq!(files.len(), 1);
        let content = fs::read_to_string(files.pop().unwrap()).unwrap();
        assert!(content.contains(">>> First entry\r\n"));
        assert!(content.contains(">>> Second entry\r\n"));

        let _ = fs::remove_dir_all(&dir);
    }
}
