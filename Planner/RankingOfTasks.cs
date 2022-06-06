﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner
{
    public class RankingOfTasks
    {
        public static Task[] RankingByImportance(ref Task[] listTasks)
        {
            var orderedList = from task in listTasks
                              orderby task.importance descending
                              select task;

            int i = 0;
            foreach (Task task in orderedList)
            {
                listTasks[i] = task;
                i++;
            }

            return listTasks;
        }

        public static Task[] RankingByDeadLine(ref Task[] listTasks)
        {
            var orderedList = from task in listTasks
                              orderby task.dataDeadline descending
                              select task;

            int i = 0;
            foreach (Task task in orderedList)
            {
                listTasks[i] = task;
                i++;
            }

            return listTasks;
        }
    }
}
