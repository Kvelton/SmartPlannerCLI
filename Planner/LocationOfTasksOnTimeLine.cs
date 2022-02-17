using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    internal class LocationOfTasksOnTimeLine
    {
        public static Task[] SettingTasks(Task[] listTasks)
        {
            Task[] TimeLine = new Task[listTasks.Length];
            TimeLine[0] = listTasks[0];

            ChekingLocationDeadline(ref listTasks,ref TimeLine);

            return TimeLine;
        }

        static void ChekingLocationDeadline (ref Task[] listTasks, ref Task[] TimeLine)
        {
            for (int i = 1; i < listTasks.Length; i++)
            {
                ChekingEnoughTime(ref listTasks[i]);
                if (listTasks[i].EnoughTime == false) { continue; }

                Task[] TemporaryTimeLine = new Task[listTasks.Length];
                for (int t = 0; t < TimeLine.Length; t++)
                {
                    TemporaryTimeLine[t]= TimeLine[t];
                }

                OffsetTask(ref TimeLine, TemporaryTimeLine, listTasks[i]);
            }
        }

        static void OffsetTask (ref Task[] TimeLine, Task[] TemporaryTimeLine, Task task)
        {
            for (int j = 0; TemporaryTimeLine[j] != null; j++)
            {
                ChekingEnoughTime(ref task);
                if (task.EnoughTime == false) { continue; }

                
                if (task.DataDeadline <= (TemporaryTimeLine[j]?.DataDeadline ?? DateTime.Now)) // Попробовать заменить условие <= на <
                {
                    if (task.Ending <= (TemporaryTimeLine[j]?.Beginning ?? DateTime.Now))
                    {
                        if ((j == 0) || (TemporaryTimeLine[j-1].Ending <= task.Beginning))
                        {
                            InsertTask(ref TimeLine, task, j);
                            break;
                        }
                        else
                        {
                            EndingInTask(ref TemporaryTimeLine[j - 1], ref task);

                            ChekingEnoughTime(ref TemporaryTimeLine[j - 1]);
                            if (TemporaryTimeLine[j - 1].EnoughTime == false) { continue; }

                            OffsetTask(ref TimeLine, TemporaryTimeLine, task);
                            break;
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
                    if (TemporaryTimeLine[j+1] == null)
                    {
                        if (task.Beginning >= (TemporaryTimeLine[j]?.Ending ?? DateTime.Now))
                        {
                            InsertTask(ref TimeLine, task, j+1);
                            break;
                        }
                        else
                        {
                            EndingInTask( ref TemporaryTimeLine[j], ref task);

                            ChekingEnoughTime(ref TemporaryTimeLine[j]);
                            if (TemporaryTimeLine[j - 1].EnoughTime == false) { continue; }

                            OffsetTask(ref TimeLine, TemporaryTimeLine, task);
                            break;
                        }
                    }
                }
            }
        }

        static void EndingInTask( ref Task newTask, ref Task oldTask)
        {
            var timeDifference = -oldTask.Beginning.Subtract(newTask.Ending);
            newTask.Ending = newTask.Ending.Subtract(timeDifference);
            newTask.Beginning = newTask.Beginning.Subtract(timeDifference);
        }
        static void InsertTask(ref Task[] TimeLine, Task task, int index)
        {
            bool offset = false;

            ChekingEnoughTime(ref task);
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

        static void ChekingEnoughTime (ref Task task)
        {
            if (task.Beginning <= DateTime.Now)
            {
                task.EnoughTime = false;
            }
        }
        
    }
}
