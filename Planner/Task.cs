using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    internal class Task
    {
        public string Name;
        public int TimeInMinutes;
        public DateTime DataDeadline;
        public byte Importance;
        public DateTime Beginning;
        public DateTime Ending;
        public bool EnoughTime = true;
        public bool Fixed = false; 

        public static void PrintActual(Task task)
        {
            if (task?.Name != null) { 
                Console.WriteLine("     Название задачи: " + task?.Name);
                Console.WriteLine("     Потребуется минут на выполнение: " + task?.TimeInMinutes);
                Console.WriteLine("     Важность задачи: " + task?.Importance);
                Console.WriteLine("     Начало задачи в: " + task?.Beginning);
                Console.WriteLine("     Конец задачи в: " + task?.Ending);
                Console.WriteLine("     Дедлайн: " + task?.DataDeadline);
                Console.WriteLine("");
            }
        }

        public static void PrintOverdue(Task task)
        {
            if (!task.EnoughTime)
            {
                Console.WriteLine("     Название задачи: " + task?.Name);
                Console.WriteLine("     Требовалось минут на выполнение: " + task?.TimeInMinutes);
                Console.WriteLine("     Дедлайн был: " + task?.DataDeadline);
                Console.WriteLine("     Важность задачи: " + task?.Importance);
                Console.WriteLine("     Дедлайн просрочен на: " + (DateTime.Now - task?.DataDeadline) );
                Console.WriteLine("");
            }
        }
    }
}
