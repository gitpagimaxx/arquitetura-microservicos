using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CartController(
    ILogger<CartController> logger,
    ICartRepository repository) : Controller
{
    private readonly ILogger<CartController> _logger = logger;
    private readonly ICartRepository _repository = repository;

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartVO>> FindById(string id)
    {
        _logger.LogInformation("Fetching cart with USERID: {id}", id);

        var cart = await _repository.FindCartByUserId(id);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
    {
        _logger.LogInformation("Add cart with VO: {id}", vo!.CartHeader!.Id);

        var cart = await _repository.SaveOrUpdateCart(vo);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
    {
        _logger.LogInformation("Update cart with VO: {id}", vo!.CartHeader!.Id);

        var cart = await _repository.SaveOrUpdateCart(vo);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<CartVO>> RemoveCart(int id)
    {
        _logger.LogInformation("Remove cart with VO: {id}", id);

        var status = await _repository.RemoveFromCart(id);
        if (!status) return NotFound();
        return Ok(status);
    }
}
