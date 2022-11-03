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
                return null;
        }
    }
}
