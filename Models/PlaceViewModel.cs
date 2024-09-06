namespace DemoVenueRental.Models
{
    public class PlaceViewModel
    {
        public PlaceInfo PlaceInfo { get; set; } = new PlaceInfo();
        public List<PlaceType> PlaceType { get; set; } = new List<PlaceType>();
        public List<PlaceImg> PlaceImgs { get; set; } = new List<PlaceImg>();
    }
}
