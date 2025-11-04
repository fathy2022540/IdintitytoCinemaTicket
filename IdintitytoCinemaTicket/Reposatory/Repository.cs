using IdintitytoCinemaTicket.Data;
using IdintitytoCinemaTicket.Reposatory.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IdintitytoCinemaTicket.Reposatory
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _Dbset;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _Dbset = _context.Set<T>();
        }

        public async Task<T> CreateAsync(T Entity, CancellationToken cancellationToken)
        {
            var entityies = await _Dbset.AddAsync(Entity);

            return entityies.Entity;
        }
        public void Update(T Entity) => _Dbset.Update(Entity);
        public void Remove(T Entity) => _Dbset.Remove(Entity);

        public async Task<IEnumerable<T>> GetAsync
            (
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            CancellationToken cancellationToken = default,
            bool Tracked = true
            )
        {
            var entityies = _Dbset.AsQueryable();

            if (expression is not null)
                entityies = entityies.Where(expression);
            if (include is not null)
            {
                foreach (var entity in include)
                {
                    entityies = entityies.Include(entity);
                }
            }
            if (!Tracked)
                entityies = entityies.AsNoTracking();
            return await entityies.ToListAsync();
        }

        public async Task<T?> GetOneAsync
            (
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>[]? include = null,
            CancellationToken cancellationToken = default,
            bool Tracked = true
            )
        {
            return (await GetAsync(expression, include, cancellationToken, Tracked)).FirstOrDefault();
        }


        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                return 0;
            }

        }
    }
}
