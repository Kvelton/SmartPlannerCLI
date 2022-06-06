using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    public class Task
    {
        public string name;
        public int timeInMinutes;
        public DateTime dataDeadline;
        public byte importance;
        public DateTime beginning;
        public DateTime ending;
        public bool enoughTime = true;
        public bool @fixed = false;

        public Task() { }

        public Task(string name, int timeInMinutes, DateTime dataDeadline, byte importance, DateTime beginning, DateTime ending)
        {
            this.name = name;
            this.timeInMinutes = timeInMinutes;
            this.dataDeadline = dataDeadline;
            this.importance = importance;
            this.beginning = beginning;
            this.ending = ending;
        }

        public static void PrintActual(Task task)
        {
            if (task?.name != null) {
                Console.WriteLine("     Название задачи: " + task?.name);
                Console.WriteLine("     Потребуется минут на выполнение: " + task?.timeInMinutes);
                Console.WriteLine("     Важность задачи: " + task?.importance);
                Console.WriteLine("     Начало задачи в: " + task?.beginning);
                Console.WriteLine("     Конец задачи в: " + task?.ending);
                Console.WriteLine("     Дедлайн: " + task?.dataDeadline);
                Console.WriteLine("");
            }
        }

        public static void PrintOverdue(Task task)
        {
            if (!task.enoughTime)
            {
                Console.WriteLine("     Название задачи: " + task?.name);
                Console.WriteLine("     Требовалось минут на выполнение: " + task?.timeInMinutes);
                Console.WriteLine("     Дедлайн был: " + task?.dataDeadline);
                Console.WriteLine("     Важность задачи: " + task?.importance);
                Console.WriteLine("     Дедлайн просрочен на: " + (DateTime.Now - task?.dataDeadline) );
                Console.WriteLine("");
            }
        }
    }
}
