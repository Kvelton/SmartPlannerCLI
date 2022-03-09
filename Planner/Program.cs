using System;
using System.Collections.Generic;

namespace Planner
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Task[] taskList = DataEntry.TaskInput();

            TaskRanking.RankingByImportance(ref taskList);

            Task[] timeLine = LocationOfTasksOnTimeLine.SortingTask(taskList);

            PrintTaskList(timeLine, "Actual");
            PrintTaskList(taskList, "Overdue");
        }

        private static void PrintTaskList(IReadOnlyList<Task> taskList, string taskType)
        {
            switch (taskType)
            {
                case "Actual":
                    {
                        Console.WriteLine("Актуальные задачи: \n");
                        for (int i = 0; i < taskList.Count; i++)
                        {
                            taskList[i].PrintActual();
                        }
                        break;
                    }
                case "Overdue":
                    {
                        Console.WriteLine("Просроченные задачи: \n");
                        for (int i = 0; i < taskList.Count; i++)
                        {
                            taskList[i].PrintOverdue();
                        }
                        break;
                    }
            }
            Console.WriteLine("-----------------------");
        }
    }
}
