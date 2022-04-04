using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    internal class LocationOfTasksOnTimeLine
    {
        public static Task[] SortingTask(Task[] listTasks)
        {
            Task[] TimeLine;

            Task BlockTime = InputDailyTimeLimit();
            TimeLine = AddBlokingTime(BlockTime, listTasks);

            OffsetTask(ref listTasks, ref TimeLine); 

            return TimeLine;
        }

        private static Task[] AddBlokingTime(Task BlockTime, Task[]listTasks)
        {
            Task[] TimeLine = new Task[listTasks.Length + LengthTimeLIne(listTasks)];

            for (int i = 0; i < LengthTimeLIne(listTasks); i++)
            {
                TimeLine[i] = new Task();
                TimeLine[i].DataDeadline = BlockTime.DataDeadline;
                TimeLine[i].Beginning = BlockTime.Beginning;
                TimeLine[i].Ending = BlockTime.Ending;
                TimeLine[i].Fixed = true;

                BlockTime.DataDeadline = BlockTime.DataDeadline.AddDays(1);
                BlockTime.Beginning = BlockTime.Beginning.AddDays(1);
                BlockTime.Ending = BlockTime.Ending.AddDays(1);
            }

            return TimeLine;
        }

        private static Task InputDailyTimeLimit()
        {
            /*            Console.WriteLine("Введите время выполнения c XX:XX");
                        Console.WriteLine("по XX:XX");

                        return DailyTimeLimit(Console.ReadLine(), Console.ReadLine());*/
            return DailyTimeLimit("17:00", "21:00");
        }

        private static void OffsetTask(ref Task[] listTasks, ref Task[] TimeLine) // переписать название функции
        {
            for (int i = 0; i < listTasks.Length; i++)
            {
                Task[] TemporaryTimeLine = CopyingArrayTasks(TimeLine);

                if (NotEnoughTimeQuestion(ref listTasks[i])) { }
                else {
                    SearchLocationTask(ref TimeLine, TemporaryTimeLine, listTasks[i]);
                }
            }
        }

        private static void SearchLocationTask(ref Task[] TimeLine, Task[] TemporaryTimeLine, Task task) // разбить её на под функции 
        {
            for (int j = 0; TemporaryTimeLine[j] != null; j++)
            {
                if (ExecutedFirstQuestion(task, TemporaryTimeLine[j]))
                {
                    if (NotOverlapQuestion(task, TemporaryTimeLine[j]))
                    {
                        if ((j == 0) || (NotOverlapQuestion( TemporaryTimeLine[j-1], task))) 
                        { 
                            InsertTask(ref TimeLine, task, j);
                            break; 
                        }
                        else
                        {   
                            if (TemporaryTimeLine[j - 1].Fixed)
                            {
                                ShiftLocationTask(ref task , ref TemporaryTimeLine[j - 1]);
                                SearchLocationTask(ref TimeLine, TemporaryTimeLine, task);
                                break;
                            }
                            else
                            {
                                ShiftLocationTask(ref TemporaryTimeLine[j - 1], ref task);
                                if (NotEnoughTimeQuestion(ref TemporaryTimeLine[j - 1])) { continue; }

                                InsertTask(ref TemporaryTimeLine, task, j - 1);

                                task = TemporaryTimeLine[j];
                                TemporaryTimeLine[j] = null;
                                SearchLocationTask(ref TimeLine, TemporaryTimeLine, TemporaryTimeLine[j - 1]);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ShiftLocationTask(ref task, ref TemporaryTimeLine[j]);
                        SearchLocationTask(ref TimeLine, TemporaryTimeLine, task);
                        break;
                    }
                }
                else
                {
                    if (TemporaryTimeLine[j + 1] == null)
                    {   
                        if (NotOverlapQuestion(TemporaryTimeLine[j], task))
                        { 
                            InsertTask(ref TimeLine, task, j + 1); 
                            break; 
                        }
                        else
                        {
                            if (TemporaryTimeLine[j].Fixed)
                            {
                                ShiftLocationTask(ref task, ref TemporaryTimeLine[j]);
                                SearchLocationTask(ref TimeLine, TemporaryTimeLine, task);
                                break;
                            }
                            else
                            {
                                ShiftLocationTask(ref TemporaryTimeLine[j], ref task);
                                if (NotEnoughTimeQuestion(ref TemporaryTimeLine[j])) { continue; }

                                InsertTask(ref TemporaryTimeLine, task, j);

                                task = TemporaryTimeLine[j + 1];
                                TemporaryTimeLine[j + 1] = null;

                                SearchLocationTask(ref TimeLine, TemporaryTimeLine, task);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static bool NotOverlapQuestion(Task task1, Task task2)
        {
            if (task1.Ending <= (task2?.Beginning ?? DateTime.Now))
            {
                return true;
            }
            else {
                return false;
            }
        }

        private static bool NotEnoughTimeQuestion(ref Task task)
        {
            if (task.Beginning < DateTime.Now)
            {
                task.EnoughTime = false;
                return true;
            }
            return false;
        }
        
        private static bool ExecutedFirstQuestion(Task task , Task task2)
        {
            if((task.Ending < (task2?.Ending ?? DateTime.Now)))
            { 
                 return true;
            }

            if (task.Ending == (task2?.Ending ?? DateTime.Now)){
                if ((task.Importance >= (task2?.Importance ?? task.Importance + 1)))
                {
                    return true;
                }
            }

            return false;
        }

        private static void ShiftLocationTask(ref Task shiftingTask, ref Task fixedTask)
        {
            var timeDifference = - fixedTask.Beginning.Subtract(shiftingTask.Ending);
            shiftingTask.Ending = shiftingTask.Ending.Subtract(timeDifference);
            shiftingTask.Beginning = shiftingTask.Beginning.Subtract(timeDifference);
        }

        private static void InsertTask(ref Task[] TimeLine, Task task, int index) // Разбить функцию на под функцию
        {
            bool offset = false;

            if (task.EnoughTime == true)
            {
                Task[] TemporaryTimeLine = CopyingArrayTasks(TimeLine);

                for (int i = 0; i < TimeLine.Length; i++)
                {
                    if (i == index)
                    {
                        TimeLine[i] = task;
                        offset = true;
                        continue;
                    }

                    if (offset == true)
                    {
                        TimeLine[i] = TemporaryTimeLine[i - 1];
                        continue;
                    }

                    if ((i + 1 == TimeLine.Length) || (TimeLine[i] == null)){ break;}
                }
            }
        }

        private static Task[] CopyingArrayTasks(Task[] ArrayTasks)
        {
            Task[] newArrayTasks = new Task[ArrayTasks.Length];
            for (int i = 0; i < ArrayTasks.Length; i++)
            {
                newArrayTasks[i] = ArrayTasks[i];
            }
            return newArrayTasks;
        }

        private static Task DailyTimeLimit(string timeStart , string timeEnd)
        {
            Task BlockedTime = new Task();

            BlockedTime.Beginning = DateTime.Today;
            BlockedTime.Beginning = BlockedTime.Beginning.AddHours((Convert.ToDateTime(timeEnd)).Hour);
            BlockedTime.Beginning = BlockedTime.Beginning.AddMinutes((Convert.ToDateTime(timeEnd)).Minute);

            BlockedTime.DataDeadline = DateTime.Today;
            BlockedTime.DataDeadline = BlockedTime.DataDeadline.AddHours((Convert.ToDateTime(timeStart)).Hour);
            BlockedTime.DataDeadline = BlockedTime.DataDeadline.AddMinutes((Convert.ToDateTime(timeStart)).Minute);

            BlockedTimeInOneDay(ref BlockedTime);

            BlockedTime.Ending = BlockedTime.DataDeadline;
            BlockedTime.Fixed = true;

            return BlockedTime;
        }

        private static void BlockedTimeInOneDay(ref Task BlockedTime)
        {
            if (BlockedTime.DataDeadline < BlockedTime.Beginning)
            {
                BlockedTime.DataDeadline = BlockedTime.DataDeadline.AddDays(1);
            }
        }
        private static int LengthTimeLIne(Task[] listTask)
        {
            RankingOfTasks.RankingByDeadLine(ref listTask);

            int Difference = (int)(listTask[0].DataDeadline - DateTime.Now.AddDays(-1)).TotalDays; 
            if (Difference < 0)
            {
                Difference = 0;
            }
            return Difference;
        }
    }
}
