using Students.Interfaces.Base.Entities;

namespace Students.DAL.Entities.Base
{
    public abstract class NamedEntity : Entity, INamedEntity
    {
        public string Name { get; set; }
    }
}
