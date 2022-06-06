using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Planner
{
    public class WriteData
    {
        public static void WriteTask(Task[] tasks)
        {
            string locationOfInputTasks = @"D:\C#\Планировщик\EntryTask.txt";

            using (StreamWriter writer = new StreamWriter(locationOfInputTasks, false))
            {
                for (int i = tasks.Length-1; i >= 0; i--)
                {
                    if (tasks[i]?.name != null)
                    {
                        writer.WriteLine(PrepareTaskForWriter(tasks[i]));
                    }
                }
            }
        }

        public static string PrepareTaskForWriter(Task task)
        {
            string stringTask;

            stringTask = task.name + "|" + task.timeInMinutes + "|" + task.dataDeadline.ToShortDateString() + "|" + task.dataDeadline.ToShortTimeString() + "|" + task.importance;

            return stringTask;
        }

    }
}
