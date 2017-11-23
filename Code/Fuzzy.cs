using System;

namespace Neolab
{
    namespace Common
    {
        /// <summary>
        /// Provides "fuzzy" values for dates, times, ...
        /// </summary>
        public static class NeoFuzzy
        {
            public static string ToFuzzyDate(this DateTime date)
            {
                string fuzzyDate = string.Empty;

                if (date.Date == DateTime.Now.Date)
                {
                    fuzzyDate = "Today";
                }
                else if (date.Date == DateTime.Now.Date.AddDays(-1))
                {
                    fuzzyDate = "Yesterday";
                }
                else
                {
                    fuzzyDate = date.ToShortDateString();
                }

                return fuzzyDate;
            }
        }
    } 
}
