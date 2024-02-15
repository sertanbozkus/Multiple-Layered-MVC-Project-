using BilgeShop.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserTypeEnum UserType { get; set; }

    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {

            builder.Property(x => x.Email)
               .HasMaxLength(50);

            builder.Property(x => x.FirstName)
                .HasMaxLength(25);

            builder.Property(x => x.LastName)
                .HasMaxLength(25);
           

            base.Configure(builder);
        }
    }


}
