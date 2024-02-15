using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _environment = environment;

        }

        public IActionResult List()
        {
            var productDtoList = _productService.GetProducts();

            var viewModel = productDtoList.Select(x => new ProductListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.CategoryName,
                ImagePath = x.ImagePath
            }).ToList();
            
            return View(viewModel);
        }

        public IActionResult New()
        {
            ViewBag.Categories = _categoryService.GetCategories();

            return View("Form", new ProductFormViewModel());
        }


        public IActionResult Update(int id)
        {
            var updateProductDto = _productService.GetProductById(id);

            var viewModel = new ProductFormViewModel()
            {
                Id = updateProductDto.Id,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                UnitPrice = updateProductDto.UnitPrice,
                UnitsInStock = updateProductDto.UnitsInStock,
                CategoryId = updateProductDto.CategoryId,
            };

            ViewBag.ImagePath = updateProductDto.ImagePath;

            ViewBag.Categories = _categoryService.GetCategories();
            return View("Form", viewModel);

        }

        public IActionResult Save(ProductFormViewModel formData)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetCategories();
                return View("Form", formData);
            }

            var newFileName = "";

            if (formData.File is not null) // dosya yüklenmek isteniyorsa
            {
                var allowedFileTypes = new string[] { "image/jpeg", "image/jpg", "image/png", "image/jfif", "image/avif" };

                var allowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png", ".jfif", ".avif" };


                var fileContentType = formData.File.ContentType;
                // dosyanın içeriği

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formData.File.FileName);
                // dosyanın uzantısız ismi

                var fileExtension = Path.GetExtension(formData.File.FileName);
                // dosyanın uzantaısı

                if(!allowedFileTypes.Contains(fileContentType) ||  
                    !allowedFileExtensions.Contains(fileExtension))
                {

                    ViewBag.ImageErrorMessage = "Yüklediğiniz dosya" + fileExtension + " uzantısında. Sisteme yalnızca .jpg .pjeg .png .jfif .avif formatında dosyalar yüklenebilir.";
                    ViewBag.Categories = _categoryService.GetCategories();
                    return View("Form", formData);
                }

                newFileName = fileNameWithoutExtension + "-" + Guid.NewGuid() + fileExtension;

                // Aynı isimde iki dosya yüklediğimizde hata almamak için her dosyayı birbiriyle asla eşleşmeyecek şekilde isimlendiriyorum. Guid : unique bir string verir.

                // Bu aşamadan sonra görseli yükleyeceğim adresi ayarlıyorum.

                var folderPath = Path.Combine("images", "products");
                // images/products

                var wwwrootFolderPath = Path.Combine(_environment.WebRootPath, folderPath);
                // .../wwwroot/images/products

                var filePath = Path.Combine(wwwrootFolderPath, newFileName);
                // .../wwwroot/images/products/urunGorsel-123123adwaw13daw.jpg

                Directory.CreateDirectory(wwwrootFolderPath); // wwwroot/images/products klasörü yoksa oluştur.

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formData.File.CopyTo(fileStream);
                    // asıl dosya kopyalamasının yapıldığı kısım.
                }
                // using içerisinde new'lenen fileStream nesnesi scope boyunca yaşar, scope bitiminde silinir.


            }

            if (formData.Id == 0) // EKLEME
            {

                var productAddDto = new ProductAddDto()
                {
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitsInStock = formData.UnitsInStock,
                    CategoryId = formData.CategoryId,
                    ImagePath = newFileName
                };

                _productService.AddProduct(productAddDto);
                return RedirectToAction("List");


            }
            else // GÜNCELLEME
            {
                var productUpdateDto = new ProductUpdateDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description,
                    UnitPrice = formData.UnitPrice,
                    UnitsInStock = formData.UnitsInStock,
                    CategoryId = formData.CategoryId,
                    ImagePath = newFileName
                };

                _productService.UpdateProduct(productUpdateDto);
                return RedirectToAction("List");

            }

          
        }

        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("List");
        }

    }
}
