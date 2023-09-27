namespace ArchyTECH.Core.Extensions
{
    public readonly struct SelectItem<TValue>
    {

        public SelectItem(string text, TValue value)
        {
            Text = text;
            Value = value;
        }
        public string Text { get; }
        public TValue Value { get; }
    }

    public static class Enums
    {
        public static IEnumerable<SelectItem<TEnum>> ToSelectItems<TEnum>() where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            foreach (var name in Enum.GetNames(enumType))
            {
                var value = (TEnum)Enum.Parse(enumType, name);

                yield return new SelectItem<TEnum>(value.DisplayName(), value);
            }
        }
    }
}
