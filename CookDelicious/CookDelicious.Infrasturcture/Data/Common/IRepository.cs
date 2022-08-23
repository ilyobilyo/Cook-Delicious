namespace CookDelicious.Infrasturcture.Data.Common
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> All<T>() where T : class;

        Task<T> GetByIdAsync<T> (object id) where T : class;

        Task<T> GetByIdsAsync<T> (object[] id) where T : class;

        Task AddAsync<T> (T entity) where T : class;

        void Update<T> (T entity) where T : class;

        void UpdateRange<T> (IEnumerable<T> entities) where T : class;

        void Delete<T> (T entity) where T : class;

        void DeleteRange<T>(IEnumerable<T> entities) where T : class;

        Task DeleteAsync<T>(object id) where T : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
