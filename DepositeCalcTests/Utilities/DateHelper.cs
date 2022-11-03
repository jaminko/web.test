using DepositeCalcTests.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepositeCalcTests.Utilities
{
    internal static class DateHelper
    {
        public static string NumberMonthName(string monthNumber)
        {
            bool isParsable = Int32.TryParse(monthNumber, out int number);

            if (isParsable)
                return CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(number);
            else
                throw new Exception($"Invalid month {number} value");
        }

        public static List<string> GetMonthDays(int year, int month)
        {
            int daysCount = DateTime.DaysInMonth(year, month);
            List<string> days = new List<string>();
            for(int i = 0; i < daysCount; i++)
            {
                days.Add(i.ToString());
            }
            return days; 
        }
    }
}
