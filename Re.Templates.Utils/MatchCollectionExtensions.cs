using System.Text.RegularExpressions;

namespace Re.Templates.Utils;

public static class MatchCollectionExtensions
{
    public static int IndexOf(this MatchCollection collection, Predicate<Match> match)
    {
        return collection.IndexOf(match, 0);
    }
    
    public static int IndexOf(this MatchCollection collection, Predicate<Match> match, int index)
    {
        for (int i = index; i < collection.Count; i++)
        {
            if (match.Invoke(collection[i]))
                return i;
        }

        return -1;
    }
}