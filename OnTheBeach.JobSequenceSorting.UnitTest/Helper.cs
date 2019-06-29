using System.Collections.Generic;

namespace OnTheBeach.JobSequenceSorting.UnitTest
{
    static class Helper
    {
        public static string ListToString(this List<string> listJobs)
        {
            return string.Join("", listJobs.ToArray());
        }
    }
}
