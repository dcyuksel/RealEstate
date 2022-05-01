namespace RealEstate.Application.Algorithms
{
    public static class RealEstateAlgorithmExtension
    {
        public static IEnumerable<TSource> TakeTopNItems<TSource, TKey>(this IEnumerable<TSource> items, int n, Func<TSource, TKey> keySelector)
        {
            var groupedItems = items.GroupBy(keySelector);
            if (n > groupedItems.Count())
            {
                throw new ArgumentException("n is higher than items size");
            }

            return groupedItems
                    .OrderByDescending(x => x.Count())
                    .Take(n)
                    .Select(x => x.First());
        }
    }
}
