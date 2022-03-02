using System;

namespace Planner
{
    internal static class LocationOfTasksOnTimeLine
    {
        public static Task[] SortingTask(Task[] listTasks)
        {
            Task blockTime = InputDailyTimeLimit();
            var timeLine = AddBlockingTime(blockTime, listTasks);

            OffsetTask(ref listTasks, ref timeLine); 

            return timeLine;
        }

        private static Task[] AddBlockingTime(Task blockTime, Task[]listTasks)
        {
            Task[] timeLine = new Task[listTasks.Length + LengthTimeLIne(listTasks)];

            for (int i = 0; i < LengthTimeLIne(listTasks); i++)
            {
                timeLine[i] = new Task
                {
                    DataDeadline = blockTime.DataDeadline,
                    Beginning = blockTime.Beginning,
                    Ending = blockTime.Ending,
                    Fixed = true
                };

                blockTime.DataDeadline = blockTime.DataDeadline.AddDays(1);
                blockTime.Beginning = blockTime.Beginning.AddDays(1);
                blockTime.Ending = blockTime.Ending.AddDays(1);
            }

            return timeLine;
        }

        private static Task InputDailyTimeLimit()
        {
            Console.WriteLine("Введите время выполнения c XX:XX");
            Console.WriteLine("по XX:XX");
  
            return DailyTimeLimit(Console.ReadLine(), Console.ReadLine());
        }

        private static void OffsetTask(ref Task[] listTasks, ref Task[] timeLine) // переписать название функции
        {
            for (int i = 0; i < listTasks.Length; i++)
            {
                Task[] temporaryTimeLine = CopyingArrayTasks(timeLine);

                if (NotEnoughTimeQuestion(ref listTasks[i])) { }
                else {
                    SearchLocationTask(ref timeLine, temporaryTimeLine, listTasks[i]);
                }
            }
        }

        private static void SearchLocationTask(ref Task[] timeLine, Task[] temporaryTimeLine, Task task) // разбить её на под функции 
        {
            for (int j = 0; temporaryTimeLine[j] != null; j++)
            {
                if (ExecutedFirstQuestion(task, temporaryTimeLine[j]))
                {
                    if (NotOverlapQuestion(task, temporaryTimeLine[j]))
                    {
                        if ((j == 0) || (NotOverlapQuestion( temporaryTimeLine[j-1], task))) 
                        { 
                            InsertTask(ref timeLine, task, j);
                            break; 
                        }

                        if (temporaryTimeLine[j - 1].Fixed)
                        {
                            ShiftLocationTask(ref task , ref temporaryTimeLine[j - 1]);
                            SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                            break;
                        }

                        ShiftLocationTask(ref temporaryTimeLine[j - 1], ref task);
                        if (NotEnoughTimeQuestion(ref temporaryTimeLine[j - 1])) { continue; }

                        InsertTask(ref temporaryTimeLine, task, j - 1);

                        task = temporaryTimeLine[j];
                        temporaryTimeLine[j] = null;
                        SearchLocationTask(ref timeLine, temporaryTimeLine, temporaryTimeLine[j - 1]);
                        break;
                    }

                    ShiftLocationTask(ref task, ref temporaryTimeLine[j]);
                    SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                    break;
                }

                if (temporaryTimeLine[j + 1] != null) continue;
                if (NotOverlapQuestion(temporaryTimeLine[j], task))
                { 
                    InsertTask(ref timeLine, task, j + 1); 
                    break; 
                }

                if (temporaryTimeLine[j].Fixed)
                {
                    ShiftLocationTask(ref task, ref temporaryTimeLine[j]);
                    SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                    break;
                }

                ShiftLocationTask(ref temporaryTimeLine[j], ref task);
                if (NotEnoughTimeQuestion(ref temporaryTimeLine[j])) { continue; }

                InsertTask(ref temporaryTimeLine, task, j);

                task = temporaryTimeLine[j + 1];
                temporaryTimeLine[j + 1] = null;

                SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                break;
            }
        }

        private static bool NotOverlapQuestion(Task task1, Task task2)
        {
            return task1.Ending <= (task2?.Beginning ?? DateTime.Now);
        }

        private static bool NotEnoughTimeQuestion(ref Task task)
        {
            if (task.Beginning >= DateTime.Now) return false;
            task.EnoughTime = false;
            return true;
        }
        
        private static bool ExecutedFirstQuestion(Task task , Task task2)
        {
            if((task.Ending < (task2?.Ending ?? DateTime.Now)))
            { 
                 return true;
            }

            if (task.Ending != (task2?.Ending ?? DateTime.Now)) return false;
            return task.Importance >= (task2?.Importance ?? task.Importance + 1);
        }

        private static void ShiftLocationTask(ref Task shiftingTask, ref Task fixedTask)
        {
            var timeDifference = - fixedTask.Beginning.Subtract(shiftingTask.Ending);
            shiftingTask.Ending = shiftingTask.Ending.Subtract(timeDifference);
            shiftingTask.Beginning = shiftingTask.Beginning.Subtract(timeDifference);
        }

        private static void InsertTask(ref Task[] timeLine, Task task, int index) // Разбить функцию на под функцию
        {
            bool offset = false;

            if (task.EnoughTime != true) return;
            Task[] temporaryTimeLine = CopyingArrayTasks(timeLine);

            for (int i = 0; i < timeLine.Length; i++)
            {
                if (i == index)
                {
                    timeLine[i] = task;
                    offset = true;
                    continue;
                }

                if (offset == true)
                {
                    timeLine[i] = temporaryTimeLine[i - 1];
                    continue;
                }

                if ((i + 1 == timeLine.Length) || (timeLine[i] == null)){ break;}
            }
        }

        private static Task[] CopyingArrayTasks(Task[] arrayTasks)
        {
            Task[] newArrayTasks = new Task[arrayTasks.Length];
            for (int i = 0; i < arrayTasks.Length; i++)
            {
                newArrayTasks[i] = arrayTasks[i];
            }
            return newArrayTasks;
        }

        private static Task DailyTimeLimit(string timeStart , string timeEnd)
        {
            Task blockedTime = new Task();

            blockedTime.Beginning = DateTime.Today;
            blockedTime.Beginning = blockedTime.Beginning.AddHours((Convert.ToDateTime(timeEnd)).Hour);
            blockedTime.Beginning = blockedTime.Beginning.AddMinutes((Convert.ToDateTime(timeEnd)).Minute);

            blockedTime.DataDeadline = DateTime.Today;
            blockedTime.DataDeadline = blockedTime.DataDeadline.AddHours((Convert.ToDateTime(timeStart)).Hour);
            blockedTime.DataDeadline = blockedTime.DataDeadline.AddMinutes((Convert.ToDateTime(timeStart)).Minute);

            BlockedTimeInOneDay(ref blockedTime);

            blockedTime.Ending = blockedTime.DataDeadline;
            blockedTime.Fixed = true;

            return blockedTime;
        }

        private static void BlockedTimeInOneDay(ref Task blockedTime)
        {
            if (blockedTime.DataDeadline < blockedTime.Beginning)
            {
                blockedTime.DataDeadline = blockedTime.DataDeadline.AddDays(1);
            }
        }
        private static int LengthTimeLIne(Task[] listTask)
        {
            RankingOfTasks.RankingByDeadLine(ref listTask);

            int difference = (int)(listTask[0].DataDeadline - DateTime.Now.AddDays(-1)).TotalDays; 
            if (difference < 0)
            {
                difference = 0;
            }
            return difference;
        }
    }
}
