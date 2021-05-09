using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Students.DAL.Entities.Base;
using Students.Interfaces.Repositories;

namespace Students.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class DbController<T> : ControllerBase where T : Entity, new()
    {
        [HttpGet("skip/{skip:int}/take/{count:int}")]
        public IActionResult Get([FromServices] IRepository<T> Items, int skip, int count) => Ok(Items.Items.Skip(skip).Take(count));

        [HttpGet("[[{Page:int}:{Count:int}]]")]
        public async Task<IActionResult> GetPage([FromServices] IRepository<T> Items, int Page, int Count) => 
            Ok(await Items.GetPageAsync(Page, Count));

        [HttpGet("count")]
        public async Task<IActionResult> GetCount([FromServices] IRepository<T> Items) =>
            Ok(await Items.GetCountAsync());

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById([FromServices] IRepository<T> Items, int Id) =>
            Ok(await Items.GetByIdAsync(Id));

        [HttpPost]
        public async Task<bool> Create([FromServices] IRepository<T> Items, T item, [FromServices] ILogger<DbController<T>> Logger)
        {
            var result = await Items.AddAsync(item) is { Id: > 0 };

            if (result)
                Logger.LogInformation("Добавлено в БД {0}", item);
            else
                Logger.LogWarning("Ошибка добавления в БД {0}", item);

            return result;
        }

        [HttpPut]
        public async Task<bool> Update([FromServices] IRepository<T> Items, T item, [FromServices] ILogger<DbController<T>> Logger)
        {
            var result = await Items.UpdateAsync(item);

            if (result is { })
                Logger.LogInformation("Обновлено в БД {0}", item);
            else
                Logger.LogWarning("Ошибка обновления в БД {0}", item);

            return result is { };
        }

        [HttpDelete("{Id:int}")]
        public async Task<bool> Delete([FromServices] IRepository<T> Items, int Id, [FromServices] ILogger<DbController<T>> Logger)
        {
            var result = await Items.DeleteByIdAsync(Id);

            if (result is { })
                Logger.LogInformation("Удалено из БД {0}", result);
            else
                Logger.LogWarning("Ошибка удаления {0} с id: {1}", typeof(T).Name, Id);

            return result is { };
        }
    }
}