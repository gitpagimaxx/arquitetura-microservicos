using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

[Route("Product")]
public class ProductController(
    ILogger<ProductController> logger,
    IProductServices productServices) : Controller
{
    private readonly ILogger<ProductController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IProductServices _productService = productServices;

    [HttpGet("")]
    public async Task<IActionResult> ProductIndex()
    {
        var products = await _productService.FindAll("");
        return View(products);
    }

    [HttpGet("create")]
    public IActionResult ProductCreate()
    {
        return View();
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> ProductCreate(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.Create(model, accessToken!);
            if (response != null) return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }

    [HttpGet("update/{id}")]
    public async Task<IActionResult> ProductUpdate(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var model = await _productService.FindById(id, accessToken!);
        if (model != null) return View(model);
        return NotFound();
    }

    [Authorize]
    [HttpPost("update")]
    public async Task<IActionResult> ProductUpdate(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.Update(model, accessToken!);
            if (response != null) return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }

    [Authorize]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var model = await _productService.FindById(id, accessToken!);
        if (model != null) return View(model);
        return NotFound();
    }

    [HttpPost("delete")]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductModel model)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var response = await _productService.Delete(model.Id, accessToken!);
        if (response) return RedirectToAction(nameof(ProductIndex));
        return View(model);
    }
}
