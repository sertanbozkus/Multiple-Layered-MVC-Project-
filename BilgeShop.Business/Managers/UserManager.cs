using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.Business.Types;
using BilgeShop.Data.Entities;
using BilgeShop.Data.Enums;
using BilgeShop.Data.Repositories;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtector _dataProtector;
        public UserManager(IRepository<UserEntity> userRepository, IDataProtectionProvider dataProtectionProvider)
        {
            _userRepository = userRepository;
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }
        public ServiceMessage AddUser(UserAddDto userAddDto)
        {

            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == userAddDto.Email.ToLower()).ToList();
            // emailleri eşleşen bütün verileri çekip listeye atadım. eğer eleman sayısı 0 ise bu mailden yok. Kayıt olunabilir. 0'dan farklı ise geriye uyarı mesajı gidecek.
            
            if(hasMail.Any()) // HasMail.Count != 0 -> eski versiyonu
            {
                return new ServiceMessage()
                {
                    IsSucceed = false,
                    Message = "Bu Eposta adresli bir kullanıcı zaten mevcut."
                };
            }

            var userEntity = new UserEntity()
            {
                FirstName = userAddDto.FirstName,
                LastName = userAddDto.LastName,
                Email = userAddDto.Email,
                Password = _dataProtector.Protect(userAddDto.Password),
                UserType = UserTypeEnum.User
            };

            _userRepository.Add(userEntity);

            return new ServiceMessage()
            {
                IsSucceed = true,
                Message = "Kayıt başarıyla tamamlandı."
            };
        }

        public UserInfoDto LoginUser(UserLoginDto userLoginDto)
        {
            var userEntity = _userRepository.Get(x => x.Email == userLoginDto.Email);

            if(userEntity is null)
            {
                return null;
                // Eğer form üzerinden gönderilen email ile eşleşen bir veri tabloda yoksa oturum açılamayacağı için geriye veri dönülmüyor.
            }

            var rawPassword = _dataProtector.Unprotect(userEntity.Password); // şifreyi açtım.

            if(rawPassword == userLoginDto.Password)
            {
                return new UserInfoDto()
                {
                    Id = userEntity.Id,
                    Email = userEntity.Email,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    UserType = userEntity.UserType,
                };
            }
            else
            {
                return null;
            }
        }
    }
}
