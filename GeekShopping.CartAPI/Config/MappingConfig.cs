using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Model;

namespace GeekShopping.CartAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps() => new(cfg =>
    {
        cfg.CreateMap<CartVO, Cart>().ReverseMap();
        cfg.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
        cfg.CreateMap<CartDetailVO, CartHeader>().ReverseMap();
        cfg.CreateMap<ProductVO, Product>().ReverseMap();
    });
}
