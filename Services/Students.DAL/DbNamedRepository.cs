using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Students.DAL.Entities.Base;
using Students.Interfaces.Repositories;

namespace Students.DAL
{
    public class DbNamedRepository<T> : DbRepository<T>, INamedRepository<T> where T : NamedEntity, new()
    {
        public DbNamedRepository(StudentsDB db) : base(db) { }

        public async Task<T> GetByNameAsync(string Name, CancellationToken Cancel = default) => Items switch
        {
            IQueryable<T> query => await query.FirstOrDefaultAsync(item => item.Name == Name, Cancel).ConfigureAwait(false),
            { } items => items.FirstOrDefault(item => item.Name == Name),
            _ => throw new InvalidOperationException()
        };

        public async Task<T> DeleteByNameAsync(string Name, CancellationToken Cancel = default)
        {
            var item = await _db.Set<T>().FirstOrDefaultAsync(i => i.Name == Name, Cancel).ConfigureAwait(false);
            if (item is null) return null;
            await DeleteAsync(item, Cancel).ConfigureAwait(false);
            return item;
        }
    }
}
