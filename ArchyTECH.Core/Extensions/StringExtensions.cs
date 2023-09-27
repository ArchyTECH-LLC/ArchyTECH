using ArchyTECH.Core.Parsing;

namespace ArchyTECH.Core.Extensions
{
    public static class StringExtensions
    {
        public static T Parse<T>(this string? input) where T : struct
        {
            T? parsedValue = input.ParseOrNull<T>();
            if (parsedValue.HasValue) return parsedValue.Value;
            throw new ArgumentException("'{0}' cannot be parsed to type '{1}' ".FormatArgs(input ?? "NULL", typeof(T).Name));
        }

        public static T? ParseOrNull<T>(this string? input) where T : struct
        {
            return StringParser.GetNullable<T>(input);
        }

        public static T ParseOrDefault<T>(this string? input, T defaultValue) where T : struct
        {
            var parsedValue = input.ParseOrNull<T>();
            return parsedValue ?? defaultValue;
        }

        public static bool HasValue(this string? input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static string? Value(this string? value)
        {
            return value?.Trim();
        }

        public static bool ContainsIgnoreCase(this string input, string value)
        {
            if (input.HasValue())
            {
                return false;
            }

            return input.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool IsNullOrEmpty(this string? input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static bool IsNullOrWhitespace(this string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
        public static bool EqualsIgnoreCase(this string? input, string input2)
        {
            return string.Equals(input, input2, StringComparison.OrdinalIgnoreCase);
        }

        public static string FormatArgs(this string? format, params object[] args)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));
            return string.Format(format, args);
        }


        public static string? NullSafeReplace(this string? value, string oldValue, string newValue)
        {
            return value?.Replace(oldValue, newValue);
        }

        public static string? FormatAsPhoneNumber(this string? phoneNumber)
        {
            if (phoneNumber.IsNullOrEmpty() || phoneNumber?.Length != 10) return phoneNumber;

            return $"({phoneNumber.Substring(0, 3)}) {phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, 4)}";

        }
    }
}