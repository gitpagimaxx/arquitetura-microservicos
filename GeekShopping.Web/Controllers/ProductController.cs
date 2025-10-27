using GeekShopping.Web.Models;
using GeekShopping.Web.Services;
using GeekShopping.Web.Services.Interfaces;
using GeekShopping.Web.Utils;
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

    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> ProductIndex()
    {
        var products = await _productService.FindAll();
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
            var response = await _productService.Create(model);
            if (response != null) return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }

    [HttpGet("update/{id}")]
    public async Task<IActionResult> ProductUpdate(int id)
    {
        var model = await _productService.FindById(id);
        if (model != null) return View(model);
        return NotFound();
    }

    [Authorize]
    [HttpPost("update")]
    public async Task<IActionResult> ProductUpdate(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _productService.Update(model);
            if (response != null) return RedirectToAction(nameof(ProductIndex));
        }
        return View(model);
    }

    [Authorize]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> ProductDelete(int id)
    {
        var model = await _productService.FindById(id);
        if (model != null) return View(model);
        return NotFound();
    }

    [HttpPost("delete")]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> ProductDelete(ProductModel model)
    {
        var response = await _productService.Delete(model.Id);
        if (response) return RedirectToAction(nameof(ProductIndex));
        return View(model);
    }
}
