using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Planner
{
    internal class DataEntry
    {
        public static Task[] DataEntryMain()
        {
            int taskCounter = 0;

            Task[] listTasks;

            string path = @"D:\C#\Планировщик\EntryTask.txt";

            using (StreamReader sr = new StreamReader(path))
            {
                listTasks = new Task[TaskCounter(sr)];
            }

            using (StreamReader sr = new StreamReader(path))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    listTasks[taskCounter] = ConversionToVar(line);
                    taskCounter++;
                }
            }

            return listTasks;
        }

        static Task ConversionToVar(string line)
        {
            string[] ArrayStringTask = line.Split(new char[] { '|' });

            Task task = new Task();

            task.Name = ArrayStringTask[0];
            task.TimeInMinutes = int.Parse(ArrayStringTask[1]);
            task.DataDeadline = Convert.ToDateTime(ArrayStringTask[2]);
            task.DataDeadline = task.DataDeadline.AddHours((Convert.ToDateTime(ArrayStringTask[3])).Hour);
            task.DataDeadline = task.DataDeadline.AddMinutes((Convert.ToDateTime(ArrayStringTask[3])).Minute);
            task.ImportanceOfTask = Convert.ToByte(ArrayStringTask[4]);
            task.Beginning = task.DataDeadline.AddMinutes(-task.TimeInMinutes);
            task.Ending = task.DataDeadline;

            return task;
        }

        static int TaskCounter(StreamReader sr)
        {
            int i = 0;

            string line;

            while ((line = sr.ReadLine()) != null)
            {
                i++;
            }

            return i;
        }
    }
}
