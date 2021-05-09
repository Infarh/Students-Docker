using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Students.Interfaces.Base.Entities;

namespace Students.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        public struct Page
        {
            public IEnumerable<T> Items { get; init; }
            public int TotalCount { get; init; }
            public int PageNumber { get; init; }
            public int Count { get; init; }

            public void Deconstruct(out IEnumerable<T> items, out int total_count) => (items, total_count) = (Items, TotalCount);
        }

        IEnumerable<T> Items { get; }

        Task<int> GetCountAsync(CancellationToken Cancel = default);

        Task<Page> GetPageAsync(int Page, int Count, CancellationToken Cancel = default);

        Task<T> GetByIdAsync(int id, CancellationToken Cancel = default);

        Task<T> AddAsync(T item, CancellationToken Cancel = default);

        Task<T> UpdateAsync(T item, CancellationToken Cancel = default);

        Task<T> DeleteAsync(T item, CancellationToken Cancel = default);

        Task<T> DeleteByIdAsync(int id, CancellationToken Cancel = default);
    }

    public interface INamedRepository<T> : IRepository<T> where T : class, INamedEntity, new()
    {
        Task<T> GetByNameAsync(string Name, CancellationToken Cancel = default);

        Task<T> DeleteByNameAsync(string Name, CancellationToken Cancel = default);
    }
}
