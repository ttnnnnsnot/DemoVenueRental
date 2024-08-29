namespace DemoVenueRental.Models
{
    public class GroundInfo
    {
        public int GroundId { get; set; }
        public int PlaceId { get; set; }
        public string NumberName { get; set; } = string.Empty;
        public int OneHourFee { get; set; }
        public bool Opening { get; set; }
        public byte StateId { get; set; }
        public byte TypeId { get; set; }
    }

    public class GroundImg
    {
        public string GroundImgId { get; set; } = string.Empty;
        public int GroundId { get; set; }
        public string imgUrl { get; set; } = string.Empty;
        public byte OrderNum { get; set; }
    }
}
