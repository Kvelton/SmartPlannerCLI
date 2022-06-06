using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Planner
{
    public class DataEntry
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

        public static Task ConversionLineToVar(string line)
        {
            string[] arrayStringElements = DivisionIntoElements(line);

            return ConvertingElements(arrayStringElements);
        }

        private static Task ConvertingElements(string[] arrayStringElements)
        {
            Task task = new Task();

            task.name = arrayStringElements[0];
            task.timeInMinutes = int.Parse(arrayStringElements[1]);
            task.dataDeadline = Convert.ToDateTime(arrayStringElements[2]);
            task.dataDeadline = task.dataDeadline.AddHours((Convert.ToDateTime(arrayStringElements[3])).Hour);
            task.dataDeadline = task.dataDeadline.AddMinutes((Convert.ToDateTime(arrayStringElements[3])).Minute);
            task.importance = Convert.ToByte(arrayStringElements[4]);
            task.beginning = task.dataDeadline.AddMinutes(-task.timeInMinutes);
            task.ending = task.dataDeadline;
            
            return task;
        }

        public static string[] DivisionIntoElements(string line)
        {
            return line.Split('|');
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
