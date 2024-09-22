namespace IODynamicObject.Application.Types.IODynamicObjects
{
    public class IODynamicObjectResponse
    {
        public long Id { get; set; }
        public string ObjectType { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public DateTime ModificationDateUtc { get; set; }
    }
}
