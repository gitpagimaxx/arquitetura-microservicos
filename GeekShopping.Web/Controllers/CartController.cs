using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class CartController(
    ILogger<CartController> logger,
    IProductService productService,
    ICartService cartService) : Controller
{
    private readonly ILogger<CartController> _logger = logger;
    private readonly IProductService _productService = productService;
    private readonly ICartService _cartService = cartService;

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await FindUserCart());
    }

    public async Task<IActionResult> Remove(long id)
    {
        if (id == 0)
        {
            return RedirectToAction(nameof(CartIndex));
        }

        var accessToken = await HttpContext.GetTokenAsync("access_token");
        if (accessToken == null)
        {
            return RedirectToAction(nameof(Remove));
        }

        var response = await _cartService.RemoveFromCart(id, accessToken!);
        
        if (response)
        {
            return RedirectToAction(nameof(CartIndex));
        }

        return View();
    }

    private async Task<CartViewModel> FindUserCart()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value!;

        var response = await _cartService.FindCartByUserId(userId, accessToken!);

        if (response?.CartHeader != null)
        {
            foreach (var detail in response.CartDetails!)
            {
                response.CartHeader.PurchaseAmount += detail.Product!.Price * detail.Count;
            }
        }

        return response!;
    }
}
