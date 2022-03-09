using System;
using System.IO;

namespace Planner
{
    public static class DataEntry
    {
        public static Task[] TaskInput()
        {
            int taskCounter = 0;
            Task[] taskList;
            string taskPath = @"D:\Учёба\Мобильные приложения\SmartPlannerCLI\tasks.txt";

            using (StreamReader sr = new StreamReader(taskPath))
            {
                taskList = new Task[TaskCounter(sr)];
            }

            using (StreamReader sr = new StreamReader(taskPath))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    taskList[taskCounter] = ConversionLineToVar(line);
                    taskCounter++;
                }
            }

            return taskList;
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
