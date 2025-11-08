using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model.Base;
using GeekShopping.ProductAPI.Model.Context;
using GeekShopping.ProductAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductAPI.Repository;

public class ProductRepository(
    MySQLContext context,
    IMapper mapper) : IProductRepository
{
    private readonly MySQLContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<ProductVO>> FindAll()
    {
        var entities = await _context.Products.ToListAsync();
        return _mapper.Map<IEnumerable<ProductVO>>(entities);
    }

    public async Task<ProductVO> FindById(long id)
    {
        var entity = 
            await _context.Products
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        
        return _mapper.Map<ProductVO>(entity);
    }

    public async Task<ProductVO> Create(ProductVO item)
    {
        var product = _mapper.Map<Product>(item);
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductVO>(product);
    }

    public async Task<ProductVO> Update(ProductVO item)
    {
        var product = _mapper.Map<Product>(item);

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductVO>(product);
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            var entity =
                await _context.Products
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (entity == null)
                return false;

            _context.Products.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
