using System;
using System.IO;
using System.Text.Json;

namespace TaskTracker.Config
{
    public class AppConfig
    {
        public string LastFilterText { get; set; } = "";
        public string LastFilterStatus { get; set; } = "Any";
        public string Role { get; set; } = "Admin"; // можно менять на User
        private string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.json");

        private void EnsureDirectory()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);
        }

        public static AppConfig Load()
        {
            var cfg = new AppConfig();
            cfg.EnsureDirectory();

            if (!File.Exists(cfg._filePath))
            {
                cfg.Save();
                return cfg;
            }

            var json = File.ReadAllText(cfg._filePath);
            return JsonSerializer.Deserialize<AppConfig>(json)!;
        }

        public void Save()
        {
            EnsureDirectory();
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}