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
            UpdateList();

            WaitingCommand();
        }

        private static void WaitingCommand()
        {
            bool stop = false;
            while (!stop)
            {

                Console.WriteLine("Выберите действие: Обновить/Удалить/Изменить/Добавить/Закончить");
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
                    case "Закончить":
                        stop = true;
                        break;

                    default:
                        Console.WriteLine("Такой команды нет, попробуйте ещё раз");
                        break;
                }

            }
        }
        private static void UpdateList()
        {
            Task[] listTasks = DataEntry.EntryTasks();

            RankingOfTasks.RankingByImportance(ref listTasks);

            Task[] timeLine = LocationOfTasksOnTimeLine.SortingTask(listTasks);

            PrintListTasks(timeLine, "Actual");
            PrintListTasks(listTasks, "Overdue");

            WriteData.WriteTask(listTasks);
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
