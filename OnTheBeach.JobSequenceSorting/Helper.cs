using System.Collections.Generic;
namespace OnTheBeach.JobSequenceSorting
{
    static class Helper
    {
        /// <summary>
        /// Extension method to convert list to string
        /// </summary>
        /// <param name="listJobs"></param>
        /// <returns></returns>
        public static string ListToString(this List<string> listJobs)
        {
            return string.Join("", listJobs.ToArray());
        }
    }
}
