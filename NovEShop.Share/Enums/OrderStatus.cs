using System.ComponentModel.DataAnnotations;

namespace NovEShop.Share.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Đang chờ xác nhận")]
        InProgress,
        [Display(Name = "Đã xác nhận")]
        Confirmed,
        [Display(Name = "Đang giao hàng")]
        Shipping,
        [Display(Name = "Giao hàng thành công")]
        Success,
        [Display(Name = "Đơn hàng đã huỷ")]
        Cancel
    }
}
