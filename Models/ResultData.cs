namespace DemoVenueRental.Models
{
    public class ResultData
    {
        public bool state { get; set; }
        public string errorMsg { get; set; } = string.Empty;
    }

    public class ResultData<T> : ResultData where T : new()
    {
        public T data { get; set; } = new T();
    }
}
