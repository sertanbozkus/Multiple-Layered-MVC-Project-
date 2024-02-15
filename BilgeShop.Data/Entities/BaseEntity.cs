using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public abstract class BaseEntity 
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }


    }

    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        // Configure metodunu miras vereceğim diğer classlarda ezmek için Virtual tanımlıyorum.
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == false);
            // Bu veritabanı üzerinde yapılacak olan bütün sorgulamalarda yukarıdaki linq geçerli olacak. Böylelikle silinmiş veriler ile uğraşmama gerek yok.

            builder.Property(x => x.ModifiedDate).IsRequired(false);
            // ModifiedDate kolonu null değer alabilir. Property'e ? eklemeyi unutma!

        }
    }

    // where TEntity : BaseEntity -> bu classın yalnızca BaseEntity tipindeki yapılarla kullanılabileceğini söylüyor.

}
