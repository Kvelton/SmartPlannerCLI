using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Planner
{
    public class Program
    {
        static void Main(string[] args)
        {
            

            WaitingCommand(UpdateList());
        }

        private static void WaitingCommand(Task[] timeLine)
        {
            bool stop = false;
            while (!stop)
            {

                Console.WriteLine("Выберите действие: Обновить/Удалить/Изменить/Добавить/Узнать свободное время/Закончить");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "Обновить":
                        UpdateList();
                        break;
                    case "Удалить":
                        DeletingTask();
                        break;
                    case "Изменить":
                        EditTask();
                        break;
                    case "Добавить":
                        AddTask();
                        break;
                    case "Узнать свободное время":
                        TimeSpan freeTime = FreeTime(timeLine, new DateTime(2022, 06, 17));
                        Console.WriteLine("Свободное время "+ freeTime.Days + "." + freeTime.Hours + ":" + freeTime.Minutes);
                        break;
                    case "Закончить":
                        stop = true;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет, попробуйте ещё раз");
                        break;
                }

            }
        }
        private static Task[] UpdateList()
        {
            Task[] listTasks = DataEntry.EntryTasks();

            RankingOfTasks.RankingByImportance(ref listTasks);

            Task[] timeLine = LocationOfTasksOnTimeLine.SortingTask(listTasks);

            Task[] overdueTasks;
            overdueTasks = OverdueTasks(listTasks, timeLine);

            PrintListTasks(timeLine, "Actual");
            PrintListTasks(overdueTasks, "Overdue");

            WriteData.WriteTask(listTasks);

            return timeLine;
        }
        
        public static Task[] OverdueTasks(Task[] listTasks, Task[] timeline)
        {
            string[] arrayNameTasks = new string[timeline.Length];
            Task[] overdueTasks = new Task[listTasks.Length];
            for (int i = 0; i < timeline.Length; i++)
            {
                arrayNameTasks[i] = timeline[i]?.name ?? "";
            }
            for (int i = 0; i < listTasks.Length; i++)
            {
                if (Array.IndexOf(arrayNameTasks, listTasks[i].name) < 0)
                {
                    listTasks[i].enoughTime = false;
                    overdueTasks[i] = listTasks[i];
                }
            }
            return overdueTasks;
        }
        private static void DeletingTask()
        {
            bool taskNotFound = true;
            Console.WriteLine("Введите название задачи которую хотите удалить:");
            string nameTask = Console.ReadLine();

            Task[] listTasks = DataEntry.EntryTasks();

            for (int i = 0; i < listTasks.Length; i++)
            {
                if (nameTask == listTasks[i].name)
                {
                    listTasks[i]=null;
                    taskNotFound = false;
                    Console.WriteLine("Задача удалена");
                }
            }
            if (taskNotFound)
            {
                Console.WriteLine("Задачи с таким названием нет");
            }

            WriteData.WriteTask(listTasks);
        }

        public static void AddTask()
        {
            List<Task> listTasks = DataEntry.EntryTasks().ToList();

            listTasks.Add(DataEntry.ConversionLineToVar(Console.ReadLine()));
            
            WriteData.WriteTask(listTasks.ToArray());
        }
        public static void EditTask()
        {

        }

        public static TimeSpan FreeTime(Task[] timeLine, DateTime day)
        {
            TimeSpan freeTime;
            int i = 0;

            while (timeLine[i].beginning < DateTime.Now)
            {
                i++;
            }
            if(i == 0)
            {
                freeTime = timeLine[i].beginning - DateTime.Now;
            }
            else
            {
                if (timeLine[i - 1].ending > DateTime.Now)
                {
                    freeTime = timeLine[i].beginning - timeLine[i - 1].ending;
                }
                else
                {
                    freeTime = timeLine[i].beginning - DateTime.Now;
                }
            }

            while (timeLine[i]?.ending < day.AddDays(1) && i != timeLine.Length-1)
            {
                if (timeLine[i] != null && timeLine[i+1] != null)
                {
                    freeTime += timeLine[i + 1].beginning - timeLine[i].ending;
                }
                i++;
            }
             
            return freeTime;
        }
        public static void PrintListTasks(Task[] listTasks, string typeTask)
        {
            switch (typeTask)
            {
                case "Actual":
                    {
                        Console.WriteLine("Актуальные задачи: \n");
                        for (int i = 0; i < listTasks.Length; i++)
                        {
                            Task.PrintActual(listTasks[i]);
                        }
                        break;
                    }
                case "Overdue":
                    {
                        Console.WriteLine("Просроченные задачи: \n");
                        for (int i = 0; i < listTasks.Length; i++)
                        {
                            Task.PrintOverdue(listTasks[i]);
                        }
                        break;
                    }
                default:
                    break;
            }
            Console.WriteLine("-----------------------");
        }
    }
}
