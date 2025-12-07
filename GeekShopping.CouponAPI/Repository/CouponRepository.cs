using AutoMapper;
using GeekShopping.CouponAPI.Data.ValueObjects;
using GeekShopping.CouponAPI.Model.Context;
using GeekShopping.CouponAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponAPI.Repository;

public class CouponRepository(
    MySQLContext context,
    IMapper mapper) : ICouponRepository
{
    private readonly MySQLContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<CouponVO> GetCouponByCode(string couponCode)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode == couponCode);

        return _mapper.Map<CouponVO>(coupon);
    }
}
