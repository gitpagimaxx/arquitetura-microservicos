using AutoMapper;
using GeekShopping.API.Data.ValueObjects;
using GeekShopping.API.Model.Base;

namespace GeekShopping.API.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps() => new(cfg =>
    {
        cfg.CreateMap<Product, ProductVO>().ReverseMap();
    });
}
