using System.Linq.Expressions;

namespace IdintitytoCinemaTicket.Reposatory.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T Entity, CancellationToken cancellationToken);
        Task<int> CommitAsync();
        Task<T?> GetOneAsync
       (
       Expression<Func<T, bool>>? expression = null,
       Expression<Func<T, object>>[]? include = null,
       CancellationToken cancellationToken = default,
       bool Tracked = true
       );
        Task<IEnumerable<T>> GetAsync
     (
     Expression<Func<T, bool>>? expression = null,
     Expression<Func<T, object>>[]? include = null,
     CancellationToken cancellationToken = default,
     bool Tracked = true
     );
        void Update(T Entity);
        void Remove(T Entity);
    }
}
