using System;

namespace Contest.Web.Common
{
    public static class Extensions
    {
        public static bool IsCurrentYear(this DateTime dateTime) =>
            dateTime.Year == DateTime.Now.Year;

        public static bool IsCurrentYear(this DateTime? dateTime) => 
            dateTime.HasValue && dateTime.Value.Year == DateTime.Now.Year;
    }
}
