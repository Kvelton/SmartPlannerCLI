using System;
using System.Collections.Generic;

namespace Planner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Task[] listTasks = DataEntry.EntryTasks();

            RankingOfTasks.RankingByImportance(ref listTasks);

            Task[] timeLine = LocationOfTasksOnTimeLine.SortingTask(listTasks);

            PrintListTasks(timeLine, "Actual");
            PrintListTasks(listTasks, "Overdue");
        }

        private static void PrintListTasks(IReadOnlyList<Task> listTasks, string typeTask)
        {
            switch (typeTask)
            {
                case "Actual":
                    {
                        Console.WriteLine("Актуальные задачи: \n");
                        for (int i = 0; i < listTasks.Count; i++)
                        {
                            listTasks[i].PrintActual();
                        }
                        break;
                    }
                case "Overdue":
                    {
                        Console.WriteLine("Просроченные задачи: \n");
                        for (int i = 0; i < listTasks.Count; i++)
                        {
                            listTasks[i].PrintOverdue();
                        }
                        break;
                    }
            }
            Console.WriteLine("-----------------------");
        }
    }
}
