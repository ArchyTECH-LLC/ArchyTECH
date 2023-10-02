using System.Collections;

namespace ArchyTECH.Core.Extensions;

/// <summary>
/// Allows multiple predicate filters to be applied to an enumerable collection without forcing multiple enumerations of the collection
/// </summary>
public class FilteredEnumerable<TItem> : IEnumerable<TItem>
{
    public FilteredEnumerable(IEnumerable<TItem> items)
    {
        Items = items;
    }
    public List<Func<TItem, bool>> Predicates { get; } = new();
    public IEnumerable<TItem> Items { get; }
    public IEnumerator<TItem> GetEnumerator()
    {
        foreach (var item in Items)
        {
            if (Predicates.All(predicate => predicate(item)))
                yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}