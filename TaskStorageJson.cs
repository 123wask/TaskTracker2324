using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TaskTracker.Models;

namespace TaskTracker.Storage
{
    public class TaskStorageJson
    {
        private string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tasks.json");

        public List<TaskItem> Load()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]"); // пустой массив JSON
                return new List<TaskItem>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json)!;
        }

        public void Save(List<TaskItem> tasks)
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}