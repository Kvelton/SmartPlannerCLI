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

            string[] blockingTimeWeek =
            {
                "12:00", "01:00",//пн
                "13:00", "16:00",//вт
                "14:00", "17:00",//ср
                "15:00", "18:00",//чт
                "16:00", "19:00",//пт
                "17:00", "20:00",//сб
                "18:00", "01:00"//вс
            };
            Task[] arrayBlockTime = DailyTimeLimit(blockingTimeWeek);
            timeLine = AddBlokingTime(arrayBlockTime, listTasks);

            OffsetTask(ref listTasks, ref timeLine);
            
            return timeLine;
        }
        

        private static Task[] AddBlokingTime(Task[] blockTime, Task[]listTasks)
        {
            Task[] timeLine = new Task[listTasks.Length + LengthTimeLIne(listTasks)];

            for (int i = 0; i < LengthTimeLIne(listTasks)+1; i++)
            {
                timeLine[i] = new Task();
                timeLine[i].dataDeadline = blockTime[(i+6)%7].dataDeadline;
                timeLine[i].beginning = blockTime[(i + 6)%7].beginning;
                timeLine[i].ending = blockTime[(i + 6) % 7].ending;
                timeLine[i].@fixed = true;

                
                Console.WriteLine(timeLine[i].beginning);
                Console.WriteLine("------------------------");
                Console.WriteLine(timeLine[i].dataDeadline);

                blockTime[(i + 6) % 7].dataDeadline = blockTime[(i + 6) % 7].dataDeadline.AddDays(7);
                blockTime[(i + 6) % 7].beginning = blockTime[(i + 6) % 7].beginning.AddDays(7);
                blockTime[(i + 6) % 7].ending = blockTime[(i + 6) % 7].ending.AddDays(7);
            }

            return timeLine;
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
        private static Task[] DailyTimeLimit(string[] arrayLimiyTime)
        {
            Task[] arrayBlokedTime = new Task[arrayLimiyTime.Length/2];
            for (int i = 0; i < arrayBlokedTime.Length; i++)
            {
                Task blockedTime = new Task();

                blockedTime.beginning = DateTime.Today.AddDays(i);
                blockedTime.dataDeadline = DateTime.Today.AddDays(i);

                if (blockedDayWeek(i) == 0)
                {
                    blockedTime.beginning = blockedTime.beginning.AddHours((Convert.ToDateTime(arrayLimiyTime[13])).Hour);
                    blockedTime.beginning = blockedTime.beginning.AddMinutes((Convert.ToDateTime(arrayLimiyTime[13])).Minute);
                }
                else
                {
                    blockedTime.beginning = blockedTime.beginning.AddHours((Convert.ToDateTime(arrayLimiyTime[blockedDayWeek(i) * 2 - 1])).Hour);
                    blockedTime.beginning = blockedTime.beginning.AddMinutes((Convert.ToDateTime(arrayLimiyTime[blockedDayWeek(i) * 2 - 1])).Minute);
                }

                blockedTime.dataDeadline = blockedTime.dataDeadline.AddHours((Convert.ToDateTime(arrayLimiyTime[blockedDayWeek(i) * 2])).Hour);
                blockedTime.dataDeadline = blockedTime.dataDeadline.AddMinutes((Convert.ToDateTime(arrayLimiyTime[blockedDayWeek(i) * 2])).Minute);

                if (i == 6)
                {
                    blockedTime.beginning = blockedTime.beginning.AddDays(-7);
                    blockedTime.dataDeadline = blockedTime.dataDeadline.AddDays(-7);
                }

                BlockedTimeInOneDay(ref blockedTime);

                blockedTime.ending = blockedTime.dataDeadline;
                blockedTime.@fixed = true;

                arrayBlokedTime[i] = blockedTime;
            }
            return arrayBlokedTime;
        }
        private static int blockedDayWeek(int i)
        {
            int index = 0;
            if (((int)DateTime.Today.DayOfWeek + 13) % 7 + i > 6)
            {
                index = ((int)DateTime.Today.DayOfWeek + 13) % 7 + i - 7;
            }
            else
            {
                index = ((int)DateTime.Today.DayOfWeek + 13) % 7 + i;
            }
            return index;
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
