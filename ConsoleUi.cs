using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Models;

namespace TaskTracker.UI
{
    public static class ConsoleUi
    {
        public static void PrintMenu()
        {
            Console.WriteLine("\nTask Tracker");
            Console.WriteLine("1) Добавить задачу");
            Console.WriteLine("2) Показать задачи");
            Console.WriteLine("4) Удалить задачу");
            Console.WriteLine("19) Расширенный поиск/фильтр");
            Console.WriteLine("20) Повторить последний фильтр");
            Console.WriteLine("21) Показать корзину");
            Console.WriteLine("22) Восстановить из корзины");
            Console.WriteLine("23) Очистить корзину (Admin)");
            Console.WriteLine("0) Выход");
            Console.WriteLine("==================================\n");
        }

        public static void PrintTasks(List<TaskItem> tasks)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список пуст.\n");
                return;
            }
            Console.WriteLine("\n--- Задачи ---");
            foreach (var t in tasks)
            {
                Console.WriteLine($"{t.Id} | {t.Title} | {t.Status}");
                if (!string.IsNullOrEmpty(t.Description))
                    Console.WriteLine($" {t.Description}");
            }
            Console.WriteLine("--------------\n");
        }
    }
}

