using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Models.Paiging
{
    public class PagingList<T> : List<T>
    {
        public PagingList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public int PageIndex { get; init; }

        public int TotalPages { get; init; }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;


        //public SortServiceModel Sorting { get; set; }

        public static async Task<PagingList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagingList<T>(items, count, pageIndex, pageSize);
        }
    }
}
