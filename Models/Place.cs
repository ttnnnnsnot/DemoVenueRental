using System.ComponentModel.DataAnnotations;

namespace DemoVenueRental.Models
{
    public class PlaceInfo
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "請輸入場所名稱")]
        [MinLength(3, ErrorMessage ="最少輸入3個字")]
        [MaxLength(30, ErrorMessage = "最多輸入30個字")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "請選擇縣市")]
        public string CityName { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入地址")]
        [MinLength(3, ErrorMessage = "最少輸入3個字")]
        [MaxLength(100, ErrorMessage = "最多輸入100個字")]
        public string Address { get; set; } = string.Empty;
        public string Describe { get; set; } = string.Empty;
        public string Rules { get; set; } = string.Empty;
        public bool Opening { get; set; } = false;
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
