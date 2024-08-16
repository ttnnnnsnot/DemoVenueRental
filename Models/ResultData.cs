using DemoVenueRental.Global;

namespace DemoVenueRental.Models
{
    public class ResultData<T> where T : new()
    {
        public bool state { get; set; }
        public string errorMsg { get; set; } = string.Empty;
        public T data { get; set; } = new T();

        public string ToJson()
        {
            return JsonSerializerService.Serialize(this);
        }
    }
}
