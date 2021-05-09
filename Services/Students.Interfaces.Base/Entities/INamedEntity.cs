using System.ComponentModel.DataAnnotations;

namespace Students.Interfaces.Base.Entities
{
    public interface INamedEntity : IEntity
    {
        [Required]
        string Name { get; }
    }
}
