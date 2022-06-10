using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    public class LocationOfTasksOnTimeLine
    {
        public static Task[] SortingTask(Task[] listTasks)
        {
            Task[] timeLine;
            

            Task blockTime = InputDailyTimeLimit();
            timeLine = AddBlokingTime(blockTime, listTasks);

            OffsetTask(ref listTasks, ref timeLine);
            
            return timeLine;
        }
        

        private static Task[] AddBlokingTime(Task blockTime, Task[]listTasks)
        {
            Task[] timeLine = new Task[listTasks.Length + LengthTimeLIne(listTasks)];

            for (int i = 0; i < LengthTimeLIne(listTasks); i++)
            {
                timeLine[i] = new Task();
                timeLine[i].dataDeadline = blockTime.dataDeadline;
                timeLine[i].beginning = blockTime.beginning;
                timeLine[i].ending = blockTime.ending;
                timeLine[i].@fixed = true;

                blockTime.dataDeadline = blockTime.dataDeadline.AddDays(1);
                blockTime.beginning = blockTime.beginning.AddDays(1);
                blockTime.ending = blockTime.ending.AddDays(1);
            }

            return timeLine;
        }

        private static Task InputDailyTimeLimit()
        {
            /*            Console.WriteLine("Введите время выполнения c XX:XX");
                        Console.WriteLine("по XX:XX");

                        return DailyTimeLimit(Console.ReadLine(), Console.ReadLine());*/
            return DailyTimeLimit("15:00", "21:00");
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
            if (NotEnoughTimeQuestion(ref task)) { }
            else{
                for (int j = 0; temporaryTimeLine[j] != null; j++)
                {
                    if (ExecutedFirstQuestion(task, temporaryTimeLine[j]))
                    {
                        if (NotOverlapQuestion(task, temporaryTimeLine[j]))
                        {
                            if ((j == 0) || (NotOverlapQuestion(temporaryTimeLine[j - 1], task)))
                            {
                                timeLine = CopyingArrayTasks(temporaryTimeLine);
                                InsertTask(ref timeLine, task, j);
                                break;
                            }
                            else
                            {
                                if (temporaryTimeLine[j - 1].@fixed)
                                {
                                    ShiftLocationTask(ref task, ref temporaryTimeLine[j - 1]);
                                    SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                                    break;
                                }
                                else
                                {
                                    ShiftLocationTask(ref temporaryTimeLine[j - 1], ref task);
                                    if (NotEnoughTimeQuestion(ref temporaryTimeLine[j - 1])) { continue; }

                                    InsertTask(ref temporaryTimeLine, task, j - 1);

                                    task = temporaryTimeLine[j];
                                    temporaryTimeLine[j] = null;
                                    SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ShiftLocationTask(ref task, ref temporaryTimeLine[j]);
                            SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                            break;
                        }
                    }
                    else
                    {
                        if (ItLastTask(temporaryTimeLine, j+1))
                        {
                            if (NotOverlapQuestion(temporaryTimeLine[j], task))
                            {
                                timeLine = CopyingArrayTasks(temporaryTimeLine);
                                InsertTask(ref timeLine, task, j + 1);
                                break;
                            }
                            else
                            {
                                if (temporaryTimeLine[j].@fixed)
                                {
                                    ShiftLocationTask(ref task, ref temporaryTimeLine[j]);
                                    SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                                    break;
                                }
                                else
                                {
                                    ShiftLocationTask(ref temporaryTimeLine[j], ref task);
                                    if (NotEnoughTimeQuestion(ref temporaryTimeLine[j])) { continue; }

                                    InsertTask(ref temporaryTimeLine, task, j);

                                    task = temporaryTimeLine[j + 1];
                                    temporaryTimeLine[j + 1] = null;

                                    SearchLocationTask(ref timeLine, temporaryTimeLine, task);
                                    break;
                                }
                            }
                        }
                    }
                }
            };
            
        }

        public static bool ItLastTask( Task[] arrayTasks, int index)
        {
            for (int i = index; i+1 < arrayTasks.Length; i++)
            {
                if(arrayTasks[i+1] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool NotOverlapQuestion(Task task1, Task task2)
        {
            if (task1.ending <= (task2?.beginning ?? DateTime.Now))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool NotEnoughTimeQuestion(ref Task task)
        {
            if (task.beginning < DateTime.Now)
            {
                task.enoughTime = false;
                return true;
            }
            return false;
        }
        
        private static bool ExecutedFirstQuestion(Task task , Task task2)
        {
            if((task.ending < (task2?.ending ?? DateTime.Now)))
            { 
                 return true;
            }

            if (task.ending == (task2?.ending ?? DateTime.Now)){
                if ((task.importance >= (task2?.importance ?? task.importance + 1)))
                {
                    return true;
                }
            }

            return false;
        }

        private static void ShiftLocationTask(ref Task shiftingTask, ref Task fixedTask)
        {
            var timeDifference = - fixedTask.beginning.Subtract(shiftingTask.ending);
            shiftingTask.ending = shiftingTask.ending.Subtract(timeDifference);
            shiftingTask.beginning = shiftingTask.beginning.Subtract(timeDifference);
        }

        private static void InsertTask(ref Task[] timeLine, Task task, int index) // Разбить функцию на под функцию
        {
            bool offset = false;

            if (task.enoughTime == true)
            {
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
        }

        public static Task[] CopyingArrayTasks(Task[] arrayTasks)
        {
            Task[] newArrayTasks = new Task[arrayTasks.Length];
            for (int i = 0; i < arrayTasks.Length; i++)
            {
                if  (arrayTasks[i] != null)
                {
                    newArrayTasks[i] = new Task(
                    arrayTasks[i]?.name,
                    arrayTasks[i].timeInMinutes,
                    arrayTasks[i].dataDeadline,
                    arrayTasks[i].importance,
                    arrayTasks[i].beginning,
                    arrayTasks[i].ending
                    );
                    newArrayTasks[i].enoughTime = arrayTasks[i].enoughTime;
                    newArrayTasks[i].@fixed = arrayTasks[i].@fixed;
                }
            }
            return newArrayTasks;
        }

        private static Task DailyTimeLimit(string timeStart , string timeEnd)
        {
            Task blockedTime = new Task();

            blockedTime.beginning = DateTime.Today;
            blockedTime.beginning = blockedTime.beginning.AddHours((Convert.ToDateTime(timeEnd)).Hour);
            blockedTime.beginning = blockedTime.beginning.AddMinutes((Convert.ToDateTime(timeEnd)).Minute);

            blockedTime.dataDeadline = DateTime.Today;
            blockedTime.dataDeadline = blockedTime.dataDeadline.AddHours((Convert.ToDateTime(timeStart)).Hour);
            blockedTime.dataDeadline = blockedTime.dataDeadline.AddMinutes((Convert.ToDateTime(timeStart)).Minute);

            BlockedTimeInOneDay(ref blockedTime);

            blockedTime.ending = blockedTime.dataDeadline;
            blockedTime.@fixed = true;

            return blockedTime;
        }

        private static void BlockedTimeInOneDay(ref Task blockedTime)
        {
            if (blockedTime.dataDeadline < blockedTime.beginning)
            {
                blockedTime.beginning = blockedTime.beginning.AddDays(-1);
            }
        }
        private static int LengthTimeLIne(Task[] listTask)
        {
            RankingOfTasks.RankingByDeadLine(ref listTask);

            int Difference = (int)(listTask[0].dataDeadline - DateTime.Now.AddDays(-2)).TotalDays; 
            if (Difference < 0)
            {
                Difference = 0;
            }
            return Difference;
        }
    }
}
