namespace Staad.Domain.Helpers
{
    using System;

    public static class DateHelper
    {
         public static string WhenItWas(this DateTime thatDate)
         {
             var now = DateTime.Now;

             if (now.Date.Equals(thatDate.Date))
             {
                 return "today";
             }
             if (now.AddDays(-1).Date.Equals(thatDate.Date))
             {
                 return "yesterday";
             }
             if (now.Month == thatDate.Month && now.Year == thatDate.Year)
             {
                 return "this month";
             }
             if (thatDate.CompareTo(now.AddMonths(-12)) >= 0)
             {
                 var howManyMonthsAgo = Math.Ceiling((double)now.Subtract(thatDate).Days / 30);
                 return string.Format("{0} months ago", (int)howManyMonthsAgo);
             }

             var howManyYearsAgo = now.Subtract(thatDate).Days / 365;
             if (howManyYearsAgo == 1)
             {
                 return "a year ago";
             }
             return string.Format("{0} years ago", howManyYearsAgo);
         }
    }
}