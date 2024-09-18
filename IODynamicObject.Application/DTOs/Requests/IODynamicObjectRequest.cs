namespace IODynamicObject.Application.DTOs.Requests
{
    public class IODynamicObjectRequest
    {
        public string ObjectType { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
