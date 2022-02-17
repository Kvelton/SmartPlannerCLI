using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Planner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task[] listTasks = DataEntry.DataEntryMain();

            RankingOfTasks.RankingByImportance(ref listTasks);

            Task[] TimeLine = LocationOfTasksOnTimeLineV2.SettingTasks(listTasks);

            PrintListTasks(TimeLine, "Actual");

            PrintListTasks(listTasks, "Overdue");
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
