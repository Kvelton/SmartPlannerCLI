using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Planner
{
    internal class WriteData
    {
         static public void WriteTask(Task[] Tasks)
        {
            string locationOfInputTasks = @"D:\C#\Планировщик\EntryTask.txt";

            using (StreamWriter writer = new StreamWriter(locationOfInputTasks, false))
            {
                for (int i = Tasks.Length-1; i >= 0; i--)
                {
                    if (Tasks[i]?.Name != null)
                    {
                        writer.WriteLine(PrepareTaskForWriter(Tasks[i]));
                    }
                }
            }
        }

        static private string PrepareTaskForWriter(Task task)
        {
            string stringTask;

            stringTask = task.Name + "|" + task.TimeInMinutes + "|" + task.DataDeadline.ToShortDateString() + "|" + task.DataDeadline.ToShortTimeString() + "|" + task.Importance;

            return stringTask;
        }

    }
}
