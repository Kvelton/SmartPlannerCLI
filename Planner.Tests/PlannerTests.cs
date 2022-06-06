using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Planner.Tests
{
    [TestClass]
    public class PlannerTests
    {
        [TestMethod]
        public void Test1()
        {
            Assert.IsNotNull(DataEntry.EntryTasks());
        }

        [TestMethod]
        public void Test2()
        {
            Task task = new Task(
                "Сделать вещь",
                10,
                new DateTime(2022, 10, 20),
                2,
                new DateTime(2022, 5, 10),
                new DateTime(2022, 7, 22)
                );
            string taskString = WriteData.PrepareTaskForWriter(task);
            Assert.AreEqual("Сделать вещь|10|20.10.2022|0:00|2", taskString);
        }

        [TestMethod]
        public void Test3()
        {
            string[] lineSplit = DataEntry.DivisionIntoElements("Сделать вещь|10|20.10.2022|0:00|2");
            Assert.AreEqual(lineSplit.Length, 5);
        }

        [TestMethod]
        public void Test4()
        {
            Task task1 = new Task(
                "1",
                1,
                new DateTime(2022, 12, 31),
                1,
                new DateTime(2021, 3, 3),
                new DateTime(2021, 9, 4)
                );
            Task task2 = new Task(
                "2",
                2,
                new DateTime(2022, 12, 31),
                2,
                new DateTime(2022, 4, 4),
                new DateTime(2022, 5, 5)
                );
            
            bool goal = LocationOfTasksOnTimeLine.NotOverlapQuestion(task1, task2);
            Assert.IsTrue(goal);
        }

        [TestMethod]
        public void Test5()
        {
            Task task1 = new Task(
                "1",
                1,
                new DateTime(2021, 12, 31),
                1,
                new DateTime(2020, 3, 3),
                new DateTime(2020, 9, 4)
                );
            bool goal = LocationOfTasksOnTimeLine.NotEnoughTimeQuestion(ref task1);
            Assert.IsTrue(goal);
        }

        [TestMethod]
        public void Test6()
        {
            Task[] taskList = { 
                new Task(
                    "1",
                    1,
                    new DateTime(2021, 12, 31),
                    1,
                    new DateTime(2020, 3, 3),
                    new DateTime(2020, 9, 4)
                    ),
                new Task(
                    "2",
                    1,
                    new DateTime(2021, 12, 31),
                    1,
                    new DateTime(2020, 3, 3),
                    new DateTime(2020, 9, 4)
                ) };
         
            Task goal = (LocationOfTasksOnTimeLine.CopyingArrayTasks(taskList)[0]);
            Assert.AreEqual(goal, taskList[0]);
        }

        [TestMethod]
        public void Test7()
        {
            Task[] taskList = {
                new Task(
                    "1",
                    1,
                    new DateTime(2021, 12, 31),
                    1,
                    new DateTime(2020, 3, 3),
                    new DateTime(2020, 9, 4)
                    ),
                new Task(
                    "2",
                    2,
                    new DateTime(2021, 12, 31),
                    2,
                    new DateTime(2020, 3, 3),
                    new DateTime(2020, 9, 4)
                ) };

            string goal = RankingOfTasks.RankingByImportance(ref taskList)[0].name;
            Assert.AreEqual(goal, "2");
        }

        [TestMethod]
        public void Test8()
        {
            Task[] taskList = {
                new Task(
                    "1",
                    1,
                    new DateTime(2021, 6, 21),
                    1,
                    new DateTime(2020, 3, 3),
                    new DateTime(2020, 9, 4)
                    ),
                new Task(
                    "2",
                    2,
                    new DateTime(2021, 10, 22),
                    2,
                    new DateTime(2020, 2, 3),
                    new DateTime(2020, 8, 4)
                ) };

            string goal = RankingOfTasks.RankingByDeadLine(ref taskList)[0].name;
            Assert.AreEqual(goal, "2");
        }
    }
}
