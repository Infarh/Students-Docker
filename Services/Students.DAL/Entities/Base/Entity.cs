using Students.Interfaces.Base.Entities;

namespace Students.DAL.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
