using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

#if NETCOREAPP
using System.ComponentModel.DataAnnotations;
#endif

namespace ArchyTECH.Core.Extensions
{
    public static class EnumExtensions
    {

        private static ConcurrentDictionary<Type, Dictionary<string, string>> EnumDisplayNameLookup { get; } = new ConcurrentDictionary<Type, Dictionary<string, string>>();

        /// <summary>
        /// Gets the display name for any enum type from the DisplayAttribute annotation or the  enum's .ToString() name
        /// </summary>
        public static string DisplayName<T>(this T item) where T : Enum
        {
            var type = typeof(T);
            var enumNameLookup = EnumDisplayNameLookup.GetOrNull(type);
            if (enumNameLookup == null)
            {


                enumNameLookup = type
                    .GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField)
                    .Select(m => new
                    {
                        m.Name,
                        Display = m.GetDisplayName()
                    })
                    .ToDictionary(m => m.Name, m => m.Display);
                EnumDisplayNameLookup[type] = enumNameLookup;
            }

            return enumNameLookup[item.ToString()];
        }

        public static string GetDisplayName(this MemberInfo memberInfo)
        {
#if NETFRAMEWORK
            return memberInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                   ?? memberInfo.Name;

#else
            return memberInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                   ?? memberInfo.GetCustomAttribute<DisplayAttribute>()?.Name
                   ?? memberInfo.Name;

#endif
        }
    }
}
