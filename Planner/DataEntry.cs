using System;
using System.IO;

namespace Planner
{
    public static class DataEntry
    {
        public static Task[] EntryTasks()
        {
            int taskCounter = 0;
            Task[] listTasks;
            string locationOfInputTasks = @"D:\Учёба\Мобильные приложения\SmartPlannerCLI\tasks.txt";

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
            Task task = new Task(arrayStringElements[0],
                int.Parse(arrayStringElements[1]),
                Convert.ToDateTime(arrayStringElements[2])
                    .AddHours(Convert.ToDateTime(arrayStringElements[3]).Hour)
                    .AddMinutes(Convert.ToDateTime(arrayStringElements[3]).Minute),
                Convert.ToByte(arrayStringElements[4]));
            return task;
        }

        private static string[] DivisionIntoElements(string line)
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
