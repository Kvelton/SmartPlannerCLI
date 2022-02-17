using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    internal class LocationOfTasksOnTimeLineV2
    {
        public static Task[] SettingTasks(Task[] listTasks)
        {
            //Console.WriteLine("Хотите ограничить время?");
            //if (Console.ReadLine() == "+")
            //{
            Console.WriteLine("Введите время выполнения c XX:XX");
            Console.WriteLine("по XX:XX");
            var BlockTime = LimitTime(Console.ReadLine(), Console.ReadLine());
            Console.WriteLine("");

            Task[] TimeLine = new Task[listTasks.Length + LengthTimeLIne(listTasks)];
            for (int i = 0; i < LengthTimeLIne(listTasks); i++) // 4 поставил, чтобы добавить покрытие блокирующего  времени всех задач, разобраться как убрать 
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

            ChekingLocationDeadline(ref listTasks, ref TimeLine);

            return TimeLine;
            //}
            //else
            //{
            //    Task[] TimeLine = new Task[listTasks.Length];

            //    ChekingLocationDeadline(ref listTasks, ref TimeLine);

            //    return TimeLine;
            //}
        }

        static void ChekingLocationDeadline(ref Task[] listTasks, ref Task[] TimeLine)
        {
            for (int i = 0; i < listTasks.Length; i++)
            {
                Task[] TemporaryTimeLine = new Task[TimeLine.Length];
                for (int t = 0; t < TimeLine.Length; t++)
                {
                    TemporaryTimeLine[t] = TimeLine[t];
                }

                OffsetTask(ref TimeLine, TemporaryTimeLine, listTasks[i]);
            }
        }

        static void OffsetTask(ref Task[] TimeLine, Task[] TemporaryTimeLine, Task task)
        {
            if (TemporaryTimeLine[0] == null)
            {
                if (task.Beginning < DateTime.Now) { task.EnoughTime = false;}

                InsertTask(ref TimeLine, task, 0);
            }
            else {
                for (int j = 0; TemporaryTimeLine[j] != null; j++)
                {
                    if (task.Beginning < DateTime.Now) { task.EnoughTime = false; continue; }

                    if (
                        (task.Ending < (TemporaryTimeLine[j]?.Ending ?? DateTime.Now))
                        ||
                        (
                        (task.Ending <= (TemporaryTimeLine[j]?.Ending ?? DateTime.Now))
                        &&
                        (task.ImportanceOfTask >= (TemporaryTimeLine[j]?.ImportanceOfTask ?? task.ImportanceOfTask+1))
                        )
                       )
                    {
                        if (task.Ending <= (TemporaryTimeLine[j]?.Beginning ?? DateTime.Now))
                        {
                            if ((j == 0) || (TemporaryTimeLine[j - 1].Ending <= task.Beginning)) { InsertTask(ref TimeLine, task, j); break; }

                            else
                            {
                                if (TemporaryTimeLine[j - 1].Fixed)
                                {
                                    EndingInTask(ref task , ref TemporaryTimeLine[j - 1]);
                                    OffsetTask(ref TimeLine, TemporaryTimeLine, task);
                                    break;
                                }
                                else
                                {
                                    EndingInTask(ref TemporaryTimeLine[j - 1], ref task);

                                    if (TemporaryTimeLine[j - 1].Beginning < DateTime.Now) { TemporaryTimeLine[j - 1].EnoughTime = false; continue; }

                                    InsertTask(ref TemporaryTimeLine, task, j - 1);

                                    task = TemporaryTimeLine[j];
                                    TemporaryTimeLine[j] = null;
                                    OffsetTask(ref TimeLine, TemporaryTimeLine, TemporaryTimeLine[j - 1]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            EndingInTask(ref task, ref TemporaryTimeLine[j]);
                            OffsetTask(ref TimeLine, TemporaryTimeLine, task);
                            break;
                        }
                    }
                    else
                    {
                        if (TemporaryTimeLine[j + 1] == null)
                        {
                            if (task.Beginning >= (TemporaryTimeLine[j]?.Ending ?? DateTime.Now)) { InsertTask(ref TimeLine, task, j + 1); break; }

                            else
                            {
                                if (TemporaryTimeLine[j].Fixed)
                                {
                                    EndingInTask(ref task, ref TemporaryTimeLine[j]);
                                    OffsetTask(ref TimeLine, TemporaryTimeLine, task);
                                    break;
                                }
                                else
                                {
                                    EndingInTask(ref TemporaryTimeLine[j], ref task);

                                    if (TemporaryTimeLine[j].Beginning < DateTime.Now) { TemporaryTimeLine[j].EnoughTime = false; continue; }


                                    InsertTask(ref TemporaryTimeLine, task, j);

                                    task = TemporaryTimeLine[j + 1];
                                    TemporaryTimeLine[j + 1] = null;
                                    OffsetTask(ref TimeLine, TemporaryTimeLine, task);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            
        }

        static void EndingInTask(ref Task newTask, ref Task oldTask)
        {
            var timeDifference = -oldTask.Beginning.Subtract(newTask.Ending);
            newTask.Ending = newTask.Ending.Subtract(timeDifference);
            newTask.Beginning = newTask.Beginning.Subtract(timeDifference);
        }
        static void InsertTask(ref Task[] TimeLine, Task task, int index)
        {
            bool offset = false;

            if (task.EnoughTime == true)
            {
                Task[] TemporaryTimeLine = new Task[TimeLine.Length];
                for (int i = 0; i < TimeLine.Length; i++)
                {
                    TemporaryTimeLine[i] = TimeLine[i];
                }

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

                    if ((i + 1 == TimeLine.Length) || (TimeLine[i] == null))
                    {
                        break;
                    }
                }
            }
        }

        static Task LimitTime(string timeStart , string timeEnd)
        {
            Task BlockedTime = new Task();

            BlockedTime.Beginning = DateTime.Today;
            BlockedTime.Beginning = BlockedTime.Beginning.AddHours((Convert.ToDateTime(timeEnd)).Hour);
            BlockedTime.Beginning = BlockedTime.Beginning.AddMinutes((Convert.ToDateTime(timeEnd)).Minute);

            BlockedTime.DataDeadline = DateTime.Today;
            BlockedTime.DataDeadline = BlockedTime.DataDeadline.AddHours((Convert.ToDateTime(timeStart)).Hour);
            BlockedTime.DataDeadline = BlockedTime.DataDeadline.AddMinutes((Convert.ToDateTime(timeStart)).Minute);

            if (BlockedTime.DataDeadline < BlockedTime.Beginning) { BlockedTime.DataDeadline = BlockedTime.DataDeadline.AddDays(1);}

            BlockedTime.Ending = BlockedTime.DataDeadline;

            BlockedTime.Fixed = true;

            return BlockedTime;
        }
        static int LengthTimeLIne(Task[] listTask)
        {
            RankingOfTasks.RankingByDeadLine(ref listTask);

            int Difference = (int)(listTask[0].DataDeadline - listTask[listTask.Length - 1].DataDeadline).TotalDays; 

            return Difference;
        }
    }
}
