using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult List()
        {
            var categoryDtoList = _categoryService.GetCategories();

            
            var viewModel = categoryDtoList.Select(x => new CategoryListViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description?.Length > 20 ? x.Description?.Substring(0, 20) + "..." : x.Description
            }).ToList();

            return View(viewModel);
        }

        public IActionResult New()
        {

            // Eğer ekleme ve güncelleme işlemleri için aynı formu kullanacaksak bu ayrım id üzerinden yapılacağından form mutlaka bir model ile açılmalı.
            return View("Form", new CategoryFormViewModel() );
        }

        public IActionResult Update(int id)
        {
            var categoryDto = _categoryService.GetCategory(id);

            var viewModel = new CategoryFormViewModel()
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            return View("Form", viewModel);
        }

        [HttpPost]
        public IActionResult Save(CategoryFormViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", formData);
            }

            if(formData.Id == 0) // EKLEME İŞLEMİ
            {
                var categoryAddDto = new CategoryAddDto()
                {
                    Name = formData.Name.Trim(),
                    Description = formData.Description?.Trim()
                    
                };

                var result = _categoryService.AddCategory(categoryAddDto);

                if (result)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut.";
                    return View("Form", formData);

                    // View dönüyorsan ViewBag çalışır.
                    // RedirectToAction ile mesaj döneceksen TempData[..] kullanmalısın.
                }
            }
            else // GÜNCELLEME İŞLEMİ
            {

                var categoryUpdateDto = new CategoryUpdateDto()
                {
                    Id = formData.Id,
                    Name = formData.Name,
                    Description = formData.Description
                };

               var result = _categoryService.UpdateCategory(categoryUpdateDto);

                if (!result)
                {
                    ViewBag.ErrorMessage = "Bu isimde bir kategori zaten mevcut olduğundan, güncelleme yapamazsınız.";
                    return View("Form", formData);
                }

                return RedirectToAction("List");

            }



            return Ok(); // Silinecek! sadece save action kızmasın diye şimdilik konuldu.

        }

        public IActionResult Delete(int id)
        {
            var result = _categoryService.DeleteCategory(id);

            if (!result)
            {
                TempData["CategoryErrorMessage"] = "İlgili kategoride ürünler bulunduğundan silme işlemi gerçekleştirilemez.";
                
            }
            return RedirectToAction("List");
        }
    }
}
