using System;

namespace Planner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task[] listTasks = DataEntry.EntryTasks();

            RankingOfTasks.RankingByImportance(ref listTasks);

            Task[] timeLine = LocationOfTasksOnTimeLine.SortingTask(listTasks);

            PrintListTasks(timeLine, "Actual");
            PrintListTasks(listTasks, "Overdue");
        }

        private static void PrintListTasks(Task[] listTasks, string typeTask)
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
            }
            Console.WriteLine("-----------------------");
        }
    }
}
