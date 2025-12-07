namespace GeekShopping.CouponAPI.Repository.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> FindAll();
    Task<T> FindById(long id);
    Task<T> Create(T item);
    Task<T> Update(T item);
    Task<bool> Delete(long id);
}
