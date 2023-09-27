using System.Runtime.InteropServices;

namespace ArchyTECH.Core.Extensions
{
    public static class FormattingExtensions
    {


        public static readonly TimeZoneInfo CentralTimeZone = 
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")
            : TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

        /// <summary>
        /// Converts UTC date to Central Standard Time timezone
        /// </summary>
        public static DateTime ToCentralTime(this DateTime dateTime)
        {

            return TimeZoneInfo.ConvertTime(dateTime, CentralTimeZone);
        }
        
        /// <summary>
        /// Converts UTC date to Central Standard Time timezone
        /// </summary>
        public static DateTime? ToCentralTime(this DateTime? dateTime)
        {
            if (dateTime == null) return null;

            return TimeZoneInfo.ConvertTime(dateTime.Value, CentralTimeZone);
        }

       
        /// <summary>
        /// Returns Yes or No
        /// </summary>
        public static string AsYesNo(this bool value) => value ? "Yes" : "No";

        /// <summary>
        /// Returns Yes, No, or null
        /// </summary>
        public static string? AsYesNo(this bool? value)
        {
            if (value == null) return null;
            return value.Value ? "Yes" : "No";
        }

        /// <summary>
        /// Displays date in MM/dd/yyyy
        /// </summary>
        /// <param name="date"></param>
        public static string? ToShortDateFormat(this DateTime? date)
        {
            return date?.ToShortDateString();
        }
        
        /// <summary>
        /// Returns a formal date format: 31st day of July 2020
        /// </summary>
        /// <param name="input">The date to format</param>
        public static string? ToFormalDateFormat(this DateTime? input)
        {
            if (input == null) return null;

            var date = input.Value;
            return $"{date.Day.ToOrdinalSuffixFormat()} day of {date:MMMM yyyy}";
        }

        /// <summary>
        /// Returns the number with it's ordinal suffix  ex: 1st, 23rd, 13th, etc
        /// </summary>
        /// <param name="input">The number to format</param>
        public static string ToOrdinalSuffixFormat(this int input)
        {
            return input switch
            {
                1 => $"{input}st",
                21 => $"{input}st",
                31 => $"{input}st",
                2 => $"{input}nd",
                22 => $"{input}nd",
                3 => $"{input}rd",
                23 => $"{input}rd",
                _ => $"{input}th"
            };
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string? ToCurrencyFormat(this double? amount, int decimalPlaces = 2)
        {
            return amount?.ToCurrencyFormat(decimalPlaces);
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string? ToCurrencyFormat(this decimal? amount, int decimalPlaces = 2)
        {
            return amount?.ToCurrencyFormat(decimalPlaces);
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string ToCurrencyFormat(this double amount, int decimalPlaces = 2)
        {
            return amount.ToString($"C{decimalPlaces}");
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string ToCurrencyFormat(this decimal amount, int decimalPlaces = 2)
        {
            return amount.ToString($"C{decimalPlaces}");
        }
        
        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string? ToNumberFormat(this int? amount)
        {
            return amount?.ToNumberFormat();
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string ToNumberFormat(this int amount)
        {
            return amount.ToString("N");
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string? ToNumberFormat(this double? amount, int decimalPlaces = 2)
        {
            return amount?.ToNumberFormat(decimalPlaces);
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string? ToNumberFormat(this decimal? amount, int decimalPlaces = 2)
        {
            return amount?.ToNumberFormat(decimalPlaces);
        }

        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string ToNumberFormat(this double amount, int decimalPlaces = 2)
        {
            return amount.ToString($"N{decimalPlaces}");
        }
      
        /// <summary>
        /// Formats the display of a currency amount using the form C standard.  1234.56 becomes $1,234.56
        /// </summary>
        /// <param name="amount">Number to convert</param>
        /// <param name="decimalPlaces">Desired decimal places (uses mid-point rounding)</param>
        public static string ToNumberFormat(this decimal amount, int decimalPlaces = 2)
        {
            return amount.ToString($"N{decimalPlaces}");
        }

    }
}
