namespace IODynamicObject.Domain.Metadata.Models
{
    public abstract class IOEntityDeletable : IOEntityTrackable
    {
        public bool Deleted { get; set; }
    }
}
