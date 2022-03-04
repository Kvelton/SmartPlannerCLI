using System;

namespace Planner
{
    public class Task
    {
        public string Name { get; set; }
        public int TimeInMinutes { get; set; }
        public DateTime DataDeadline { get; set; }
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

        public Task(string name, int timeInMinutes, DateTime dataDeadline, byte importance)
        {
            Name = name;
            TimeInMinutes = timeInMinutes;
            DataDeadline = dataDeadline;
            Importance = importance;
            Beginning = dataDeadline.AddMinutes(-timeInMinutes);
            Ending = dataDeadline;
        }

        public void PrintActual()
        {
            if (Name == null) return;
            Console.WriteLine("     Название задачи: " + Name);
            Console.WriteLine("     Потребуется минут на выполнение: " + TimeInMinutes);
            Console.WriteLine("     Важность задачи: " + Importance);
            Console.WriteLine("     Начало задачи в: " + Beginning);
            Console.WriteLine("     Конец задачи в: " + Ending);
            Console.WriteLine("     Дедлайн: " + DataDeadline);
            Console.WriteLine("");
        }

        public void PrintOverdue()
        {
            if (EnoughTime) return;
            Console.WriteLine("     Название задачи: " + Name);
            Console.WriteLine("     Требовалось минут на выполнение: " + TimeInMinutes);
            Console.WriteLine("     Дедлайн был: " + DataDeadline);
            Console.WriteLine("     Важность задачи: " + Importance);
            Console.WriteLine("     Дедлайн просрочен на: " + (DateTime.Now - DataDeadline) );
            Console.WriteLine("");
        }
    }
}
