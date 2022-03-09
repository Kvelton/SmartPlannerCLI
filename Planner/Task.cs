using System;

namespace Planner
{
    public class Task
    {
        public string Name { get; set; }
        public int TimeInMinutes { get; set; }
        public DateTime DateDeadline { get; set; }
        public byte Importance { get; set; }
        public DateTime Beginning { get; set; }
        public DateTime Ending { get; set; }
        public bool EnoughTime { get; set; }
        public bool Fixed { get; set; }

        public Task()
        {
            EnoughTime = true;
            Fixed = false;
        }

        public Task(string name, int timeInMinutes, DateTime dateDeadline, byte importance)
        {
            Name = name;
            TimeInMinutes = timeInMinutes;
            DateDeadline = dateDeadline;
            Importance = importance;
            Beginning = dateDeadline.AddMinutes(-timeInMinutes);
            Ending = dateDeadline;
        }

        public void PrintActual()
        {
            if (Name == null) return;
            Console.WriteLine("     Название задачи: " + Name);
            Console.WriteLine("     Потребуется минут на выполнение: " + TimeInMinutes);
            Console.WriteLine("     Важность задачи: " + Importance);
            Console.WriteLine("     Начало задачи в: " + Beginning);
            Console.WriteLine("     Конец задачи в: " + Ending);
            Console.WriteLine("     Дедлайн: " + DateDeadline);
            Console.WriteLine("");
        }

        public void PrintOverdue()
        {
            if (EnoughTime) return;
            Console.WriteLine("     Название задачи: " + Name);
            Console.WriteLine("     Требовалось минут на выполнение: " + TimeInMinutes);
            Console.WriteLine("     Дедлайн был: " + DateDeadline);
            Console.WriteLine("     Важность задачи: " + Importance);
            Console.WriteLine("     Дедлайн просрочен на: " + (DateTime.Now - DateDeadline) );
            Console.WriteLine("");
        }
    }
}
