using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OnTheBeach.JobSequenceSorting.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Print_NonDependent_SingleJob_Success()
        {
            var jobs = new List<string> { "a" };
            var result = Program.Process(jobs);

            Assert.AreEqual(result, jobs);

            // f c b a d
        }

        [TestMethod]
        public void Print_NonDependent_MultipleJob_Success()
        {
            var jobs = new List<string> { "a", "b", "c", "d" };
            var result = Program.Process(jobs);
            Assert.AreEqual(result, jobs);

        }

        [TestMethod]
        public void Print_Dependent_MultipleJob_Success()
        {
            var jobs = new List<string> { "a", "b>c", "c" };
            var result = Program.Process(jobs);
            var convertedJobs = Helper.ListToString(result);
            var expectedResponse = "acb";
            Assert.AreEqual(convertedJobs, expectedResponse);

        }


        [TestMethod]
        public void Print_Dependent_SelfReferenceJob_Exception()
        {
            var jobs = new List<string> { "a", "b", "c>c" };
            Assert.ThrowsException<ArgumentException>(() => Program.Process(jobs));

        }


        [TestMethod]
        public void Print_Dependent_MultipleJobs_Success()
        {
            var jobs = new List<string> { "a", "b>c", "c>f", "d>a", "e>b", "f" };
            var result = Program.Process(jobs);
            var convertedJobs = Helper.ListToString(result);
            var expectedResponse = "afbcde";
            Assert.AreEqual(convertedJobs, expectedResponse);

        }


    }
    static class Helper
    {
        public static string ListToString(this List<string> l)
        {
            return string.Join("", l.ToArray());
        }
    }
}
