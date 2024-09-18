using System.ComponentModel.DataAnnotations;

namespace IODynamicObject.Domain.Metadata.Models
{
    public abstract class IOEntityBase
    {
        [Key]
        public virtual long Id { get; set; }
    }
}
