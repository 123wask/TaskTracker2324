using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = TaskTracker.Models.TaskStatus;
using TaskTracker.Models;

namespace TaskTracker.Services
{
    public class TaskService
    {
        private List<TaskItem> _tasks;
        private int _nextId;

        public TaskService(List<TaskItem> tasks)
        {
            _tasks = tasks;
            _nextId = _tasks.Count == 0 ? 1 : _tasks.Max(t => t.Id) + 1;
        }

        public void Add(string title, string desc)
        {
            var task = new TaskItem
            {
                Id = _nextId++,
                Title = title,
                Description = desc,
                Status = TaskStatus.New
            };
            _tasks.Add(task);
        }

        public List<TaskItem> GetAll() => _tasks;

        public List<TaskItem> GetAllActive() => _tasks.Where(t => !t.IsDeleted).ToList();

        public TaskItem GetExisting(int id)
        {
            var t = _tasks.FirstOrDefault(x => x.Id == id);
            if (t == null) throw new ArgumentException($"Задача Id={id} не найдена.");
            return t;
        }

        public void Delete(int id)
        {
            var t = GetExisting(id);
            if (t.IsDeleted) throw new ArgumentException($"Задача Id={id} уже в корзине.");
            t.IsDeleted = true;
        }

        public void Restore(int id)
        {
            var t = GetExisting(id);
            if (!t.IsDeleted) throw new ArgumentException($"Задача Id={id} не в корзине.");
            t.IsDeleted = false;
        }

        public int ClearTrash()
        {
            int before = _tasks.Count;
            _tasks.RemoveAll(t => t.IsDeleted);
            _nextId = _tasks.Count == 0 ? 1 : _tasks.Max(x => x.Id) + 1;
            return before - _tasks.Count;
        }

        public List<TaskItem> GetTrash() => _tasks.Where(t => t.IsDeleted).ToList();

        public List<TaskItem> SearchAdvanced(string? text, TaskStatus? status)
        {
            var query = (text ?? "").Trim();
            bool hasText = query.Length > 0;
            var result = new List<TaskItem>();

            foreach (var t in _tasks)
            {
                if (t.IsDeleted) continue;
                bool ok = true;

                if (status.HasValue && t.Status != status.Value) ok = false;
                if (ok && hasText)
                {
                    bool inTitle = (t.Title ?? "").Contains(query, StringComparison.OrdinalIgnoreCase);
                    bool inDesc = (t.Description ?? "").Contains(query, StringComparison.OrdinalIgnoreCase);
                    if (!inTitle && !inDesc) ok = false;
                }

                if (ok) result.Add(t);
            }
            return result;
        }
    }
}