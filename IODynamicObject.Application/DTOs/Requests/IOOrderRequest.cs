namespace IODynamicObject.Application.DTOs.Requests
{
    public class IOOrderRequest
    {
        public Dictionary<string, object> OrderData { get; set; }
        public List<Dictionary<string, object>> OrderItems { get; set; }
    }
}
