using GeekShopping.CouponAPI.Data.ValueObjects;
using GeekShopping.CouponAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CouponController(
    ILogger<CouponController> logger,
    ICouponRepository repository) : Controller
{
    private readonly ILogger<CouponController> _logger = logger;
    private readonly ICouponRepository _repository = repository;

    [HttpGet]
    [Route("{couponCode}")]
    public async Task<ActionResult<CouponVO>> Index(string couponCode)
    {
        _logger.LogInformation("Finding coupon {coupon}", couponCode);

        var coupon = await _repository.GetCouponByCode(couponCode);
        if (coupon == null) return NotFound();
        return Ok(coupon);
    }
}
