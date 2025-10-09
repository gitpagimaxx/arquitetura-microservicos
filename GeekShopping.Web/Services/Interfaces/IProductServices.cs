using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Interfaces;

public interface IProductServices
{
    Task<IEnumerable<ProductModel>> FindAll();
    Task<ProductModel> FindById(long id);
    Task<ProductModel> Create(ProductModel model);
    Task<ProductModel> Update(ProductModel model);
    Task<bool> Delete(long id);
}
