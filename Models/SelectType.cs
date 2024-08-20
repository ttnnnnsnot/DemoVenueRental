namespace DemoVenueRental.Models
{
    public class SelectType
    {
        public string typeName { get; set; } = string.Empty;
        public string iconClass { get; set; } = "fa-solid fa-medal";
        public string defaultText { get; set; } = "運動項目";
        public List<SelectData> listItem { get; set; } = new List<SelectData>();
    }

    public class SelectData
    {
        public int selectTypeId { get; set; }
        public string name { get; set; } = string.Empty;
    }
}
