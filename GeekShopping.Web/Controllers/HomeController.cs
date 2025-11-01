using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeekShopping.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IProductServices productServices) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IProductServices _productService = productServices;

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

    public IActionResult Privacy()
    {
        return View();
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
