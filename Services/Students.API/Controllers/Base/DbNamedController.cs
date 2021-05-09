using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Students.DAL.Entities.Base;
using Students.Interfaces.Repositories;

namespace Students.API.Controllers.Base
{
    public abstract class DbNamedController<T> : DbController<T> where T : NamedEntity, new()
    {
        [HttpGet("name/{Name}")]
        public async Task<IActionResult> GetById([FromServices] INamedRepository<T> Items, string Name) =>
            Ok(await Items.GetByNameAsync(Name));
    }
}
