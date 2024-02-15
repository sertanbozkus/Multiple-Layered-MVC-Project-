using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public ProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }


        public IViewComponentResult Invoke(int? categoryId = null)
        {
            var productDtos = _productService.GetProductsByCategoryId(categoryId);

            var viewModel = productDtos.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryName = x.CategoryName,
                UnitsInStock = x.UnitsInStock,
                UnitPrice = x.UnitPrice,
                ImagePath = x.ImagePath
            }).ToList();

            return View(viewModel);
        }
    }
}
