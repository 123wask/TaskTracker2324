using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Models
{
    public enum TaskStatus
    {
        New = 1,
        InProgress = 2,
        Done = 3
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public TaskStatus Status { get; set; } = TaskStatus.New;
        public bool IsDeleted { get; set; } = false;
    }
}
