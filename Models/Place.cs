using System.ComponentModel.DataAnnotations;

namespace DemoVenueRental.Models
{
    public class PlaceInfo
    {
        public int UserId { get; set; }

        [MaxLength(30, ErrorMessage = "最多輸入30個字")]
        public string? Name { get; set; }
        public int CityId { get; set; }

        [MaxLength(100, ErrorMessage = "最多輸入100個字")]
        public string? Address { get; set; }
        public string? Describe { get; set; }
        public string? Rules { get; set; }
        public bool Opening { get; set; }
        public byte StateId { get; set; } = 1;
    }
    public class PlaceInfoType
    {
        public int PlaceId { get; set; }
        public byte TypeId { get; set; }
    }
    public class PlaceImg
    {
        public string PlaceImgId { get; set; } = string.Empty;
        public int PlaceId { get; set; }
        public string imgUrl { get; set; } = string.Empty;
        public byte OrderNum { get; set; }
    }
}
