namespace DemoVenueRental.Models
{
    public class SelectType
    {
        public string iconClass { get; set; } = "fa-solid fa-medal";
        public string defaultText { get; set; } = "運動項目";
        public string[] listItem { get; set; } = ["羽球", "籃球", "桌球"];
    }
}
