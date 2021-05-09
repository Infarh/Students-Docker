using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Students.DAL.Entities.Base;
using Students.Interfaces.Repositories;

namespace Students.DAL
{
    public class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly StudentsDB _db;

        protected virtual DbSet<T> Set { get; }

        public DbRepository(StudentsDB db)
        {
            _db = db;
            Set = db.Set<T>();
        }

        public IEnumerable<T> Items => Set;

        public async Task<int> GetCountAsync(CancellationToken Cancel = default) => await Set.CountAsync(Cancel).ConfigureAwait(false);

        public async Task<IRepository<T>.Page> GetPageAsync(int Page, int Count, CancellationToken Cancel = default) =>
            Items switch
            {
                IQueryable<T> query => new IRepository<T>.Page
                {
                    Items = query.OrderBy(i => i.Id).Skip(Page * Count).Take(Count),
                    TotalCount = await query.CountAsync(Cancel).ConfigureAwait(false),
                    PageNumber = Page,
                    Count = Count
                },
                { } items => new IRepository<T>.Page
                {
                    Items = items.Skip(Page * Count).Take(Count),
                    TotalCount = items.Count(),
                    PageNumber = Page,
                    Count = Count
                },
                _ => throw new InvalidOperationException()
            };

        public async Task<T> GetByIdAsync(int id, CancellationToken Cancel = default) => Items switch
        {
            DbSet<T> set => await set.FindAsync(new object[] { id }, Cancel).ConfigureAwait(false),
            IQueryable<T> query => await query.FirstOrDefaultAsync(item => item.Id == id, Cancel).ConfigureAwait(false),
            { } items => items.FirstOrDefault(item => item.Id == id),
            _ => throw new InvalidOperationException()
        };

        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            _db.Entry(item).State = EntityState.Added;
            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<T> UpdateAsync(T item, CancellationToken Cancel = default)
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<T> DeleteAsync(T item, CancellationToken Cancel = default)
        {
            _db.Entry(item).State = EntityState.Deleted;
            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<T> DeleteByIdAsync(int id, CancellationToken Cancel = default)
        {
            var item = await _db.Set<T>().FindAsync(new object[] { id }, Cancel).ConfigureAwait(false);
            if (item is null) return null;
            await DeleteAsync(item, Cancel).ConfigureAwait(false);
            return item;
        }
    }
}
