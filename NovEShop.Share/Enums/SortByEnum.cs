using System.ComponentModel.DataAnnotations;

namespace NovEShop.Share.Enums
{
    public enum SortByEnum
    {
        [Display(Name = "Mặc định")]
        Default = 0,
        [Display(Name = "Phổ biến")]
        Popularity = 1,
        [Display(Name = "Mới nhất")]
        Newest = 2,
        [Display(Name = "Giá thấp tới cao")]
        PriceLowToHigh = 3,
        [Display(Name = "Giá cao tới thấp")]
        PriceHighToLow = 4,
    }
}
