using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lm.Comol.Core.DomainModel.Helpers
{
    public static class CacheExpiration
    {
        public static TimeSpan _2minutes { get { return new TimeSpan(0, 2, 0); } }
        public static TimeSpan _5minutes { get { return new TimeSpan(0, 5, 0); } }
        public static TimeSpan _20minutes { get { return new TimeSpan(0, 20, 0); } }
        public static TimeSpan _30minutes { get { return new TimeSpan(0, 30, 0); } }
        public static TimeSpan _Hour { get { return new TimeSpan(1, 0, 0); } }
        public static TimeSpan _12hours { get { return new TimeSpan(12, 0, 0); } }
        public static TimeSpan Minutes(int minutes)
        {
            return new TimeSpan(0, minutes, 0);
        }
        public static TimeSpan Hours(int hours)
        {
            return new TimeSpan(hours, 0, 0);
        }
        public static TimeSpan Day { get { return new TimeSpan(1, 0, 0, 0); } }
        public static TimeSpan Days(int days)
        {
            return new TimeSpan(days, 0, 0, 0);
        }
        public static TimeSpan Week { get { return new TimeSpan(7, 0, 0, 0); } }
        public static TimeSpan Weeks(int weeks)
        {
            return new TimeSpan(7 * weeks, 0, 0, 0);
        }
        public static TimeSpan Month { get { return new TimeSpan(30, 0, 0, 0); } }
        public static TimeSpan Months(int months)
        {
            return new TimeSpan(30 * months, 0, 0, 0);
        }
        public static TimeSpan Semester { get { return new TimeSpan(180, 0, 0, 0); } }
        public static TimeSpan Year { get { return new TimeSpan(365, 0, 0, 0); } }
        public static TimeSpan Years(int years)
        {
            return new TimeSpan(365 * years, 0, 0, 0);
        }
    }
}