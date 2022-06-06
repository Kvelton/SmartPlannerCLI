using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Planner.Tests
{
    [TestClass]
    public class PlannerTests
    {
        [TestMethod]
        public void EntryTasks()
        {
            Assert.IsNotNull(DataEntry.EntryTasks());
        }

        [TestMethod]
        public void PrepareTaskForWriter()
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
        public void DivisionIntoElements()
        {
            string[] lineSplit = DataEntry.DivisionIntoElements("Сделать вещь|10|20.10.2022|0:00|2");
            Assert.AreEqual(lineSplit.Length, 5);
        }

        [TestMethod]
        public void NotOverlapQuestion()
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
    }
}
