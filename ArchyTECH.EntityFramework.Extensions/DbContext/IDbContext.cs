namespace ArchyTECH.EntityFramework.Extensions.DbContext
{
    public interface IDbContext : IDisposable
    {
        void AddEntity<T>(T entity) where T : class;
        void RemoveEntity<T>(T entity) where T : class;
        IQueryable<T> Query<T>() where T : class;
        //void SaveChanges(); // Leaving this for the app to decide save signature
    }
}