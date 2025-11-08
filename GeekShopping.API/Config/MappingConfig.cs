using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model.Base;

namespace GeekShopping.ProductAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps() => new(cfg =>
    {
        cfg.CreateMap<Product, ProductVO>().ReverseMap();
    });
}
