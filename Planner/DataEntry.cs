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
        public static Task[] EntryTasks()
        {
            int taskCounter = 0;
            Task[] listTasks;
            string locationOfInputTasks = @"D:\C#\Планировщик\EntryTask.txt";

            using (StreamReader sr = new StreamReader(locationOfInputTasks))
            {
                listTasks = new Task[TaskCounter(sr)];
            }

            using (StreamReader sr = new StreamReader(locationOfInputTasks))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    listTasks[taskCounter] = ConversionLineToVar(line);
                    taskCounter++;
                }
            }

            return listTasks;
        }

        private static Task ConversionLineToVar(string line)
        {
            string[] arrayStringElements = DivisionIntoElements(line);

            return ConvertingElements(arrayStringElements);
        }

        private static Task ConvertingElements(string[] arrayStringElements)
        {
            Task task = new Task();

            task.Name = arrayStringElements[0];
            task.TimeInMinutes = int.Parse(arrayStringElements[1]);
            task.DataDeadline = Convert.ToDateTime(arrayStringElements[2]);
            task.DataDeadline = task.DataDeadline.AddHours((Convert.ToDateTime(arrayStringElements[3])).Hour);
            task.DataDeadline = task.DataDeadline.AddMinutes((Convert.ToDateTime(arrayStringElements[3])).Minute);
            task.Importance = Convert.ToByte(arrayStringElements[4]);
            task.Beginning = task.DataDeadline.AddMinutes(-task.TimeInMinutes);
            task.Ending = task.DataDeadline;
            
            return task;
        }

        private static string[] DivisionIntoElements(string line)
        {
            return line.Split(new char[] { '|' });
        }

        private static int TaskCounter(StreamReader sr)
        {
            int i = 0;

            while (sr.ReadLine() != null)
            {
                i++;
            }

            return i;
        }
    }
}
