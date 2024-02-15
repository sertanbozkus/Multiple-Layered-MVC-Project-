using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class CategoryManager : ICategoryService
    {
        private readonly IRepository<CategoryEntity> _categoryRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        public CategoryManager(IRepository<CategoryEntity> categoryRepository, IRepository<ProductEntity> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;

        }
        public bool AddCategory(CategoryAddDto categoryAddDto)
        {
           


            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == categoryAddDto.Name.ToLower()).ToList();

            if(hasCategory.Any()) // hasCategory.Count != 0
            {
                return false;
                // Bu isimde kategori zaten mevcutsa, ekleme yapmayacağımdan geriye false dönüyorum.
            }


            var entity = new CategoryEntity()
            {
                Name = categoryAddDto.Name,
                Description = categoryAddDto.Description
            };

            _categoryRepository.Add(entity);
            return true;
        }

        public bool DeleteCategory(int id)
        {
            var firstProduct = _productRepository.Get(x => x.CategoryId == id);

            if(firstProduct is not null)
            {
                return false;
                //silme işlemi yapılamaz, içerisinde en az 1 ürün var.
            }

            _categoryRepository.Delete(id);
            return true;
        }

        public List<CategoryListDto> GetCategories()
        {
            var categoryEntities = _categoryRepository.GetAll().OrderBy(x => x.Name);

            var categoryDtoList = categoryEntities.Select(x => new CategoryListDto()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
            // Her bir entity için 1 adet CategoryListDto newliyor. veri aktarımı gerçekleştirip listeye çeviriyor.

            return categoryDtoList;

        }

        public CategoryUpdateDto GetCategory(int id)
        {
            var entity = _categoryRepository.GetById(id);

            var categoryUpdateDto = new CategoryUpdateDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            return categoryUpdateDto;

        }

        public bool UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var hasCategory = _categoryRepository.GetAll(x => x.Name.ToLower() == categoryUpdateDto.Name.ToLower() && x.Id != categoryUpdateDto.Id).ToList();
            // Kendisi hariç aynı isimde başka veriyle eşleşiyorsa listeye çekiyorum. Bunu yapmamdaki neden ismi aynı tutup diğer özellikleri değiştirmek istediğimizde kendi verisini çekip zaten mevcut dememesi için.
            

            if (hasCategory.Any()) // hasCategory.Count != 0
            {
                return false;
                // Aynı isimde kategori olduğundan o isme güncelleme yapmıyorum.
            }

            var entity = _categoryRepository.GetById(categoryUpdateDto.Id);

            entity.Name = categoryUpdateDto.Name;
            entity.Description = categoryUpdateDto.Description;

            _categoryRepository.Update(entity);
            return true;

        }
    }
}
