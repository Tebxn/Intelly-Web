using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface IProductModel
    {

        Task<ApiResponse<List<ProductEnt>>> GetAllProducts();
        Task<ApiResponse<ProductEnt>> AddProduct(ProductEnt entity);
        Task<ApiResponse<ProductEnt>> GetSpecificProduct(int ProductId);
        Task<ApiResponse<ProductEnt>> EditSpecificProduct(ProductEnt entity);
    }
}
