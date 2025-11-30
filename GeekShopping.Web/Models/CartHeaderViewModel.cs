namespace GeekShopping.Web.Models;

public class CartHeaderViewModel
{
    public long Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CouponCode { get; set; } = string.Empty;
    public decimal PurchaseAmount { get; set; }
}
