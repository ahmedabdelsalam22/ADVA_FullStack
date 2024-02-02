using System.Linq.Expressions;

namespace ADVA_API.RepositoryPattern.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, bool tracked = true, string[]? includes = null);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, string[]? includes = null);
        Task Create(T entity);
        Task Delete(T entity);
    }
}
