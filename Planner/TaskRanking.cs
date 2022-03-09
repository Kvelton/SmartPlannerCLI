using System.Linq;

namespace Planner
{
    public static class TaskRanking
    {
        public static void RankingByImportance(ref Task[] taskList)
        {
            var orderedList = from task in taskList
                              orderby task.Importance descending
                              select task;

            int i = 0;
            foreach (Task task in orderedList)
            {
                taskList[i] = task;
                i++;
            }

            //return taskList;
        }

        public static void RankingByDeadLine(ref Task[] taskList)
        {
            var orderedList = from task in taskList
                              orderby task.DateDeadline descending
                              select task;

            int i = 0;
            foreach (Task task in orderedList)
            {
                taskList[i] = task;
                i++;
            }

            //return taskList;
        }
    }
}
