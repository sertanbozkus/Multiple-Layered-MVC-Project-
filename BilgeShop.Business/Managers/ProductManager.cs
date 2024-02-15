using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductManager(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        public void AddProduct(ProductAddDto productAddDto)
        {

            var entity = new ProductEntity()
            {
                Name = productAddDto.Name,
                Description = productAddDto.Description,
                UnitPrice = productAddDto.UnitPrice,
                UnitsInStock = productAddDto.UnitsInStock,
                CategoryId = productAddDto.CategoryId,
                ImagePath = productAddDto.ImagePath,
            };

            _productRepository.Add(entity);

        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public ProductUpdateDto GetProductById(int id)
        {
            var entity = _productRepository.GetById(id);

            var productUpdateDto = new ProductUpdateDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UnitPrice = entity.UnitPrice,
                UnitsInStock = entity.UnitsInStock,
                CategoryId = entity.CategoryId,
                ImagePath = entity.ImagePath,
            };

            return productUpdateDto;
        }

        public List<ProductListDto> GetProducts()
        {
            var productEntites = _productRepository.GetAll().OrderBy(x => x.Category.Name).ThenBy(x => x.Name);
            // Önce kategori adına sonra kategori içinde ürün isimlerine göre sıralıyorum.

            var productDtoList = productEntites.Select(x => new ProductListDto()
            {
                Id = x.Id,
                Name = x.Name,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                CategoryName = x.Category.Name,
                ImagePath = x.ImagePath
            }).ToList();

            return productDtoList;

        }

        public List<ProductListDto> GetProductsByCategoryId(int? categoryId = null)
        {
            if(categoryId.HasValue) // is not null
            {
                var productEntities = _productRepository.GetAll(x => x.CategoryId == categoryId).OrderBy(x => x.Name);
                // Gönderdiğim categoryId ile entity'deki categoryId'si eşleşen ürünleri isimlerine göre sıralayarak getir.

                var productDtos = productEntities.Select(x => new ProductListDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnitPrice = x.UnitPrice,
                    UnitsInStock = x.UnitsInStock,
                    CategoryName = x.Category.Name,
                    ImagePath = x.ImagePath
                }).ToList();

                return productDtos;
            }

            return GetProducts(); // Aynı işlemler yapılacağından direkt diğer metoda yönlendiriyorum.


        }

        public void UpdateProduct(ProductUpdateDto productUpdateDto)
        {
            var entity = _productRepository.GetById(productUpdateDto.Id);

            entity.Name = productUpdateDto.Name;
            entity.Description = productUpdateDto.Description;
            entity.UnitPrice = productUpdateDto.UnitPrice;
            entity.UnitsInStock = productUpdateDto.UnitsInStock;
            entity.CategoryId = productUpdateDto.CategoryId;

            if(productUpdateDto.ImagePath != "")
            entity.ImagePath = productUpdateDto.ImagePath;
            // Bu if'i yazmazsam, productUpdateDto ile View'den gelen boş olan string ImagePath bilgisi, veritabanındaki görsel üzerinde yazılır. Bu durumda görseli kaybederim. ISTERSEN IF'I YORUM SATIRI YAPIP BIR DENE !

            _productRepository.Update(entity);

        }

      
    }
}
