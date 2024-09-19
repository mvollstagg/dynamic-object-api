using System.ComponentModel.DataAnnotations;

namespace IODynamicObject.Core.Metadata.Models
{
    public abstract class IOEntityBase
    {
        [Key]
        public virtual long Id { get; set; }
    }
}
