namespace CookDelicious.Core.Service.Models
{
    public class PagedListServiceModel<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }
    }
}
