namespace IODynamicObject.Core.Metadata.Models
{
    public abstract class IOEntityDeletable : IOEntityTrackable
    {
        public bool Deleted { get; set; }
    }
}
