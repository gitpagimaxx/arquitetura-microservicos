using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Model.Context;
using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository;

public class CartRepository(
    MySQLContext context,
    IMapper mapper) : ICartRepository
{
    private readonly MySQLContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeader = await _context.CartHeaders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader is not null)
        {
            _context.CartDetails
                .RemoveRange(_context.CartDetails
                    .Where(cd => cd.CartHeaderId == cartHeader.Id));

            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<CartVO> FindCartByUserId(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId)
        };

        cart.CartDetails = _context.CartDetails
            .AsNoTracking()
            .Where(cd => cd.CartHeaderId == cart.CartHeader!.Id)
            .Include(cd => cd.Product);

        return _mapper.Map<CartVO>(cart);
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveFromCart(long cartDetailId)
    {
        try
        {
            CartDetail cartDetail = await _context.CartDetails
                    .FirstOrDefaultAsync(c => c.Id == cartDetailId);

            int total = _context.CartDetails
                .Where(c => c.CartHeaderId == cartDetail.CartHeaderId).Count();

            _context.CartDetails.Remove(cartDetail);

            if (total == 1)
            {
                var cartHeaderToRemove = await _context.CartHeaders
                    .FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                _context.CartHeaders.Remove(cartHeaderToRemove);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<CartVO> SaveOrUpdateCart(CartVO cartVO)
    {
        try
        {
            Cart cart = _mapper.Map<Cart>(cartVO);

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == cart.CartDetails!.First().ProductId);

            if (product == null)
            {
                _context.Products.Add(cart.CartDetails!.FirstOrDefault()!.Product!);
                await _context.SaveChangesAsync();
            }

            CartHeader? cartHeader = await _context.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader!.UserId);

            if (cartHeader == null)
            {
                _context.CartHeaders.Add(cart.CartHeader!);
                await _context.SaveChangesAsync();
                cart.CartDetails!.First().CartHeaderId = cart.CartHeader!.Id;
                cart.CartDetails!.First().Product = null; // Avoid re-inserting the product
                _context.CartDetails.Add(cart.CartDetails!.First());
                await _context.SaveChangesAsync();
            }
            else
            {
                CartDetail? cartDetail = await _context.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        cd => cd.ProductId == cart.CartDetails!.First().ProductId &&
                              cd.CartHeaderId == cartHeader.Id);
                
                if (cartDetail == null)
                {
                    cart.CartDetails!.FirstOrDefault()!.CartHeaderId = cartHeader!.Id;
                    cart.CartDetails!.FirstOrDefault()!.Product = null;
                    _context.CartDetails.Add(cart.CartDetails!.FirstOrDefault()!);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails!.FirstOrDefault()!.Product = null;
                    cart.CartDetails!.FirstOrDefault()!.Count += cartDetail.Count;
                    cart.CartDetails!.FirstOrDefault()!.Id = cartDetail.Id;
                    cart.CartDetails!.FirstOrDefault()!.CartHeaderId = cartDetail.CartHeaderId;
                    _context.CartDetails.Update(cart.CartDetails!.FirstOrDefault()!);
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartVO>(cart);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
