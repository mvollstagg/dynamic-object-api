namespace IODynamicObject.Domain.Metadata.Models
{
    public abstract class IOEntityTrackable : IOEntityBase
    {
        public DateTime CreationDateUtc { get; set; }
        public long CreatedById { get; set; }
        public DateTime ModificationDateUtc { get; set; }
        public long ModificatedById { get; set; }
    }
}
