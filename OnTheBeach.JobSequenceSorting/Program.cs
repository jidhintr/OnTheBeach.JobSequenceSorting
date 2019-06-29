using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OnTheBeach.JobSequenceSorting
{

    /// <summary>
    /// Details and assumptions are in REadMe.txt and proper unit test for all combinations are written 
    /// </summary> 
    public class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Welcome to job logical job ordering");

            // Assuming the jobs are 1 letter in length
            // Assuming split character as > with 1 length
            // More details in ReadMe.Txt file

            var jobLists = new List<string> { "a", "b>c", "c>f", "d>a", "e>b", "f" };

            Console.Write("Unsorted job list are : ");
            Console.WriteLine(string.Join(", ", jobLists.ToArray()));
            //jobLists.ForEach(Console.Write);
            var sortedJobList = Process(jobLists);

            Console.Write("Logically sorted list is :");

            Console.ForegroundColor = ConsoleColor.Green;
            sortedJobList.ForEach(Console.Write);
            var finalPrintableResult = Helper.ListToString(sortedJobList);
            Console.WriteLine("Thank you for using the tool. Press any keys to exit");
            Console.ReadLine();


        }




        /// <summary>
        /// Process the job order and sort as per logical occurrence 
        /// </summary>
        /// <param name="jobLists">List of jobs with or without dependency</param>
        /// <returns>Return the string of jobs as per their logical occurrence order</returns>
        public static List<string> Process(List<string> jobLists)
        {
            // check the jobs has dependency, if not print in the order as it is
            // assuming 'job' is a single char like 'a' with length =1 , from question 
            // if input one job-item is like 'a=>' length check value will be changed accordingly  
            if (jobLists.All(job => job.Length == 1))
            {
                return jobLists;
            }
            if (jobLists.All(a => string.IsNullOrEmpty(a)))
            {
                return new List<string> { string.Empty };
            }

            var tempOutputList = new List<string>();
            var nonDependentJobs = jobLists.Where(a => a.Length == 1);
            tempOutputList.AddRange(nonDependentJobs);
            var dependedntJobs = jobLists.Where(a => a.Length > 1);
            var distinctJobs = dependedntJobs.Select(a => a.Split('>')).ToList();

            // throw exception if same job is in dependent to itself.
            #region Validation and ErrorMEssages
            SelfReferenceChecking(distinctJobs);
            #endregion

            var dependedntJob = dependedntJobs.Select(a => a.Split('>').LastOrDefault()).ToList();
            foreach (var job in dependedntJob)
            {
                // Main extraction engine of priority and secondary jobs 
                string priorJob = ExtractPriorJobFromList(dependedntJobs, job);
                string secondaryJob = ExtractSecondaryJobs(dependedntJobs, job);

                // validation check
                CheckCircularJob(dependedntJobs, priorJob);

                // add to list of jobs in order, if not already exists 
                FillOutputList(tempOutputList, priorJob, secondaryJob);
            }
            return tempOutputList;

        }




        /// <summary>
        /// Fill the output of jobs into list
        /// </summary>
        /// <param name="tempOutputList">List to add</param>
        /// <param name="priorJob">job that should add first before the dependency job</param>
        /// <param name="secondaryJob">succeeder job that to be added after the prior one  </param>
        private static void FillOutputList(List<string> tempOutputList, string priorJob, string secondaryJob)
        {
            if (!string.IsNullOrEmpty(priorJob) && !tempOutputList.Contains(priorJob))
                tempOutputList.Add(priorJob);

            if (!string.IsNullOrEmpty(secondaryJob) && !tempOutputList.Contains(secondaryJob))
                tempOutputList.Add(secondaryJob);
        }




        #region Core-Process

        /// <summary>
        /// To find the prior job from the list , with its dependent job 
        /// </summary>
        /// <param name="dependedntJobs"></param>
        /// <param name="job"></param>
        /// <returns>Prior job as per the logical orders</returns>
        private static string ExtractPriorJobFromList(IEnumerable<string> dependedntJobs, string job)
        {
            return dependedntJobs.Where(a => a.StartsWith(job)).Select(s => s.Split('>').LastOrDefault()).FirstOrDefault();
        }

        private static string ExtractSecondaryJobs(IEnumerable<string> dependedntJobs, string job)
        {
            return dependedntJobs.Where(a => a.EndsWith(job)).Select(s => s.Split('>').FirstOrDefault()).FirstOrDefault();
        }


        #endregion

        #region Validation-Checks


        /// <summary>
        /// To find the jobs has any circular dependency
        /// </summary>
        /// <param name="dependedntJobs">List of jobs to loop through dependency</param>
        /// <param name="priorJob">The dependent job is to be checked against jobs</param>
        private static void CheckCircularJob(IEnumerable<string> dependedntJobs, string priorJob)
        {

            if (!string.IsNullOrEmpty(priorJob))
            {// search job with inputKey, and split to take the last portion ie : dependent job
             // get list of connected jobs, we can either choose 'FirstItem() also, but for n params i select List and loop
                var circularJobsCheckList = dependedntJobs.Where(a => a.StartsWith(priorJob)).
                    Select(a => a.Split('>').LastOrDefault()).ToList();
                foreach (var item in circularJobsCheckList)
                { // check the dependent job in the list of Jobs, of true throw exception.
                    var circularJob = dependedntJobs.Any(a => a.StartsWith(item));
                    if (circularJob)
                        throw new ArgumentException($"Circular dependency found for the job '{item}'. Check the inputs");

                }
            }


        }

        /// <summary>
        // Checking self reference for jobs
        /// </summary>
        /// <param name="distinctJobs">Jobs list to check unique data</param>
        private static void SelfReferenceChecking(List<string[]> distinctJobs)
        {//check the distinct jobs in the list are same with count of lists
            if (distinctJobs.Any(a => a.Distinct().Count() != a.Count()))
                throw new ArgumentException("Same job dependency found. Check the input jobs provided");
        }


        #endregion



        #region DEpricated-Tests
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

        #endregion
    }
}
