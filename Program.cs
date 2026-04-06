using System;
using TaskTracker.Services;
using TaskTracker.UI;
using TaskTracker.Config;
using TaskTracker.Storage;
using TaskTracker.Models;

// alias для TaskStatus
using TaskStatus = TaskTracker.Models.TaskStatus;

var storage = new TaskStorageJson();
var tasks = storage.Load();
var service = new TaskService(tasks);

var cfg = AppConfig.Load();

string lastText = cfg.LastFilterText;
TaskStatus? lastStatus = null;
if (cfg.LastFilterStatus != "Any" && Enum.TryParse<TaskStatus>(cfg.LastFilterStatus, out var parsed))
    lastStatus = parsed;

while (true)
{
    ConsoleUi.PrintMenu();
    Console.Write("Выбор: ");
    var input = (Console.ReadLine() ?? "").Trim();

    if (input == "1")
    {
        Console.Write("Название: "); var title = Console.ReadLine() ?? "";
        Console.Write("Описание: "); var desc = Console.ReadLine() ?? "";
        service.Add(title, desc);
        storage.Save(service.GetAll());
        Console.WriteLine("Задача добавлена.\n");
    }
    else if (input == "2")
    {
        var active = service.GetAllActive();
        ConsoleUi.PrintTasks(active);
    }
    else if (input == "4")
    {
        Console.Write("Id: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                service.Delete(id);
                storage.Save(service.GetAll());
                Console.WriteLine($"Задача Id={id} отправлена в корзину.\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n"); }
        }
    }
    else if (input == "19")
    {
        Console.Write("Текст для поиска (Enter = пусто): ");
        var text = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine("Статус:");
        Console.WriteLine("0 - Любой"); Console.WriteLine("1 - New");
        Console.WriteLine("2 - InProgress"); Console.WriteLine("3 - Done");
        Console.Write("Выбор: "); var s = (Console.ReadLine() ?? "").Trim();

        TaskStatus? status = null;
        if (s == "1") status = TaskStatus.New;
        else if (s == "2") status = TaskStatus.InProgress;
        else if (s == "3") status = TaskStatus.Done;

        var filtered = service.SearchAdvanced(text, status);
        ConsoleUi.PrintTasks(filtered);

        lastText = text;
        lastStatus = status;
        cfg.LastFilterText = lastText;
        cfg.LastFilterStatus = lastStatus?.ToString() ?? "Any";
        cfg.Save();
    }
    else if (input == "20")
    {
        Console.WriteLine("Повтор последнего фильтра:");
        var filtered = service.SearchAdvanced(lastText, lastStatus);
        ConsoleUi.PrintTasks(filtered);
    }
    else if (input == "21")
    {
        var trash = service.GetTrash();
        Console.WriteLine("Корзина:");
        ConsoleUi.PrintTasks(trash);
    }
    else if (input == "22")
    {
        var trash = service.GetTrash();
        ConsoleUi.PrintTasks(trash);

        Console.Write("Введите Id для восстановления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            try
            {
                service.Restore(id);
                storage.Save(service.GetAll());
                Console.WriteLine($"Задача Id={id} восстановлена.\n");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message + "\n"); }
        }
    }
    else if (input == "23")
    {
        if (cfg.Role != "Admin") { Console.WriteLine("Очистка доступна только Admin.\n"); continue; }

        Console.Write("Удалить все задачи из корзины? (y/n): ");
        if ((Console.ReadLine() ?? "").Trim().ToLower() == "y")
        {
            int removed = service.ClearTrash();
            storage.Save(service.GetAll());
            Console.WriteLine($"Корзина очищена. Удалено: {removed}\n");
        }
        else Console.WriteLine("Очистка отменена.\n");
    }
    else if (input == "0") break;
    else Console.WriteLine("Неверная команда.\n");
}