using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekShopping.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IProductService productService,
    ICartService cartService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IProductService _productService = productService;
    private readonly ICartService _cartService = cartService;

    public async Task<IActionResult> Index()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var products = await _productService.FindAll(accessToken!);
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var model = await _productService.FindById(id, accessToken!);
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPost(ProductViewModel model)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        
        CartViewModel cart = new()
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value!,
            }
        };

        CartDetailViewModel cartDetail = new()
        {
            Count = model.Count,
            ProductId = model.Id,
            Product = await _productService.FindById(model.Id, accessToken!),
        };

        List<CartDetailViewModel> cartDetails = [cartDetail];

        cart.CartDetails = cartDetails;

        var response = await _cartService.AddItemToCart(cart, accessToken!);

        if (response != null)
        {
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        if (accessToken == null) {
            return Challenge();
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}
