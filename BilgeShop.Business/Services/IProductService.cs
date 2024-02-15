using BilgeShop.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Services
{
    public interface IProductService
    {
        void AddProduct(ProductAddDto productAddDto);

        List<ProductListDto> GetProducts();

        ProductUpdateDto GetProductById(int id);

        void UpdateProduct(ProductUpdateDto productUpdateDto);

        void DeleteProduct(int id);

        List<ProductListDto> GetProductsByCategoryId(int? categoryId = null);
    }
}
