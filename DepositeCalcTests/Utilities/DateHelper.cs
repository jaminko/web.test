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
        public static string NumberOfMonthToNameOfMonth(string monthNumber)
        {
            bool isParsable = Int32.TryParse(monthNumber, out int number);

            if (isParsable)
                return CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(number);
            else
                throw new Exception($"Invalid month number: {number}");
        }

        public static List<string> GetMonthDays(int year, int month)
        {
            int daysCount = DateTime.DaysInMonth(year, month);
            List<string> days = new List<string>();
            for(int i = 1; i <= daysCount; i++)
            {
                days.Add(i.ToString());
            }
            return days; 
        }
    }
}
