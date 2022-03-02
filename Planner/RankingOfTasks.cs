using System.Linq;

namespace Planner
{
    internal static class RankingOfTasks
    {
        public static Task[] RankingByImportance(ref Task[] listTasks)
        {
            var orderedList = from task in listTasks
                              orderby task.Importance descending
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
                              orderby task.DataDeadline descending
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
