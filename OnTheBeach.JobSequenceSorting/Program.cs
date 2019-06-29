using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OnTheBeach.JobSequenceSorting
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to job logical job ordering");

            //var jobLists = new List<string> { "a", "b", "c", "f" };
            //var jobLists = new List<string> { "a", "b>c", "c>f", "d>a", "e>b", "f" };
            var jobLists = new List<string> { "a", "b>c", "c>f", "d>a", "e>b", "f" };

            Console.Write("Unsorted job list are : "); jobLists.ForEach(Console.Write);
            var sortedJobList = Process(jobLists);
            Console.WriteLine();
            Console.Write("Logically sorted list is :");

            Console.ForegroundColor = ConsoleColor.Green;
            sortedJobList.ForEach(Console.Write);

            Console.ReadLine();


        }

        public static List<string> Process(List<string> jobLists)
        {
            // check the jobs has dependency, if not print in the order as it is
            // assuming 'job' is a single char like 'a' with length =1 , from question 
            // if input one job-item is like 'a=>' length check value will be changed accordingly  
            if (jobLists.All(job => job.Length == 1))
            {
                //jobLists.ForEach(Console.Write);
                return jobLists;
            }


            var tempOutputList = new List<string>();
            var nonDependentJobs = jobLists.Where(a => a.Length == 1);
            tempOutputList.AddRange(nonDependentJobs);
            var dependedntJobs = jobLists.Where(a => a.Length > 1);
            var sp = dependedntJobs.Select(a => a.Split('>')).ToList();

            // throw exception if same job is in dependent to itself.
            #region Validation and ErrorMEssages

            //     if (sp.All(a => a.Distinct().Count() != a.Count())) throw new ArgumentException($"Same job is dependency found for {sp.FirstOrDefault()}");
            if (sp.Any(a => a.Distinct().Count() != a.Count())) throw new ArgumentException("Same job dependency found. Check the input jobs provided");

            #endregion

            var dependedntJob = dependedntJobs.Select(a => a.Split('>').LastOrDefault()).ToList();
            foreach (var job in dependedntJob)
            {

                var priorJob = dependedntJobs.Where(a => a.StartsWith(job)).Select(s => s.Split('>').LastOrDefault()).FirstOrDefault();
                var secondaryJob = dependedntJobs.Where(a => a.EndsWith(job)).Select(s => s.Split('>').FirstOrDefault()).FirstOrDefault();

                // add to list if not exists 
                if (!string.IsNullOrEmpty(priorJob) && !tempOutputList.Contains(priorJob))
                    tempOutputList.Add(priorJob);
                if (!string.IsNullOrEmpty(secondaryJob) && !tempOutputList.Contains(secondaryJob))
                    tempOutputList.Add(secondaryJob);
            }

            return tempOutputList;

        }

        /// <summary>
        /// To swap the dependent job. 
        /// </summary>
        /// <param name="item">Assuming 'a>b' , where job 'b' should finish before 'a' </param>
        /// <param name="spliter">character to split the string </param>
        /// <returns>Returns 'b>a' reverse in the logical execution order</returns>
        private static string ReArrangeDependentJobs(string item, char spliter = '>')
        {
            //split the dependent jobs for swapping logical orders
            var jobs = item.Split(spliter).ToList();

            // throw exception if same job is in dependent to itself.
            if (jobs.Distinct().Count() != jobs.Count)
            {
                throw new ArgumentException($"Same job is dependency found for {jobs.FirstOrDefault()}");
            }
            var lastIndex = jobs.LastOrDefault();
            var firstIndex = jobs.FirstOrDefault();

            //use stringBuilder than string concatenations,  
            var sb = new StringBuilder();
            // filtering only job items with length > 1, for ordering. 
            // non-dependent job can be placed anywhere in the sequence  //  item.Where(a => a.Length= 1)
            return jobs.Count > 1 ? sb.Append(jobs[1]).Append(jobs[0]).ToString() : item;

        }
    }
}
