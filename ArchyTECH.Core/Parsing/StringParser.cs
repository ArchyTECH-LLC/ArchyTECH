using System.Text.RegularExpressions;
using ArchyTECH.Core.Extensions;

namespace ArchyTECH.Core.Parsing
{
    public static class StringParser
    {
        private delegate bool TryParseDelegate<T>(string? s, out T result);
        private static readonly Dictionary<Type, object> ParsingDelegateCache = new();
        private static readonly Regex TrueExpression = new("^(true|yes|1|on|y)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex FalseExpression = new("^(false|no|0|off|n)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Type GuidType = typeof(Guid);
        private static readonly Delegate ParseGuidDelegate = TryParseGuid;

        private static readonly Type BoolType = typeof(bool);
        private static readonly Delegate ParseBoolDelegate = TryParseBool;
        
        /// <summary>
        /// Returns a Nullable instance that is parsed from the input string.
        /// </summary>
        /// <typeparam name="T">The Nullable type to parse the value for.</typeparam>
        /// <param name="s">The value to parse for creating the nullable value</param>
        /// <returns>Null if the input string was null or could not be parsed.  The value of
        /// the parsed string if the parsing was successful.</returns>
        public static T? GetNullable<T>(string? s)
        {
            return Parse(s, out T? value) ? value : default;
        }

        /// <summary>
        /// Attempts to parse the string into the Nullable structure of type T.
        /// </summary>
        /// <typeparam name="T">The structure to try and parse the input string into.</typeparam>
        /// <param name="s">The input string to parse.</param>
        /// <param name="result">The Nullable result of the parse operation.</param>
        /// <param name="method">The TryParseDelegate that is to be invoked to attempt the parsing.</param>
        /// <returns>True if the string was successfully parsed into the structure; otherwise false.</returns>
        /// <remarks>If the input string is null or empty, method returns true and result.HasValue will
        /// equal false.  If the input string is not in the correct format, method returns false and
        /// result.HasValue will equal false.  If the input string is in the correct format and is not
        /// null or empty, method returns true and result.HasValue will equal true.</remarks>
        private static bool TryParse<T>(string? s, out T? result, TryParseDelegate<T> method)
        {
            if (s.HasValue())
            {
                var success = method(s, out var value);
                if (success)
                {
                    result = value;
                    return true;
                }
            }

            result = default;
            return false;
        }

        /// <summary>
        /// Method to attempt to parse a string into a Guid result.
        /// </summary>
        /// <param name="s">The string to attempt to parse into a Guid.</param>
        /// <param name="result">The resulting Guid.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        private static bool TryParseGuid(string? s, out Guid result)
        {
            if (string.IsNullOrEmpty(s))
            {
                //can't parse so return false.  Still have to assign a value to the return, so
                //set result to Guid.Empty
                result = Guid.Empty;
                return false;
            }
            try
            {
                //try parsing.  If we can create a Guid from the string, return that Guid.
                result = new Guid(s.Trim());
                return true;
            }
            catch (FormatException)
            {
                //can't parse so return false.  Still have to assign a value to the return, so
                //set result to Guid.Empty
                result = Guid.Empty;
                return false;
            }
        }

        /// <summary>
        /// Method to attempt to parse a string into a bool result.
        /// </summary>
        /// <param name="s">The string to attempt to parse into a bool.</param>
        /// <param name="result">The resulting bool.</param>
        /// <returns>True if the parsing was successful; otherwise false.</returns>
        private static bool TryParseBool(string s, out bool result)
        {
            if (string.IsNullOrEmpty(s))
            {
                //can't parse so return false.  Still have to assign a value to the return,
                //so set the result to false
                result = false;
                return false;
            }
            //try matching the true and false expressions
            if (TrueExpression.IsMatch(s))
            {
                result = true;
                return true;
            }
            if (FalseExpression.IsMatch(s))
            {
                result = false;
                return true;
            }
            result = false;
            return false;
        }

        private static bool TryParseEnum<T>(string? s, out T result)
        {

            try
            {

#if NETCOREAPP
                if (Enum.TryParse(typeof(T), s, true, out var parsedObj))
                {
                    result = (T)parsedObj;
                    return true;
                }
#else

                if (s != null)
                {
                    result = (T)Enum.Parse(typeof(T), s, true);
                    return true;
                }
#endif
            }
            catch
            {
                // .NET Framework does not provide a exception safe try parse method

                // ignored
            }

            result = default!;
            return false;
        }

        /// <summary>
        /// Parses the input string and creates a Nullable struct from it.
        /// </summary>
        /// <typeparam name="T">The struct to try and create from the string reference.</typeparam>
        /// <param name="s">The string to parse into the structure.</param>
        /// <param name="result">The nullable result.</param>
        /// <returns>True if the input string was successfully parsed; otherwise false.</returns>
        /// <remarks>result.HasValue will be false if the input string was not successfully parsed 
        /// or if the input string is null or empty. The parsing will fail if the input string is 
        /// not in the correct format.  Parsing succeeds if the input string is null or empty.</remarks>
        private static bool Parse<T>(string? s, out T? result)
        {
            TryParseDelegate<T>? parseDelegate;
            var resultType = typeof(T);

            //see if we've already resolved the delegate for the type that's being passed in
            //if we have, then use that delegate instead of continually using reflection to find it
            lock (ParsingDelegateCache)
            {
                parseDelegate = (TryParseDelegate<T>?)ParsingDelegateCache.GetOrNull(resultType);

                if (parseDelegate == null)
                {
                    parseDelegate = CreateParsingDelegate<T>(resultType);

                    //add the delegate to the dictionary of resolved delegates_delegates[typeof(T)] = parseDelegate;
                    ParsingDelegateCache[resultType] = parseDelegate;
                }
            }

            return TryParse(s, out result, parseDelegate);
        }

        private static TryParseDelegate<T> CreateParsingDelegate<T>(Type resultType)
        {
            // GUIDs have a TryParseGuid method instead of TryParse
            if (resultType == GuidType)
            {
                return (TryParseDelegate<T>)ParseGuidDelegate;
            }

            // Enhancing Boolean parsing to be for flexible for 0/1, yes/no etc
            if (resultType == BoolType)
            {
                return (TryParseDelegate<T>)ParseBoolDelegate;
            }

            // Enable parsing enums
            if (typeof(T).IsEnum)
            {
                return TryParseEnum;
            }

            // Default: attempt to find a TryParse method for this type
            return (TryParseDelegate<T>)
                Delegate.CreateDelegate(typeof(TryParseDelegate<T>), typeof(T), "TryParse");

        }
    }
}