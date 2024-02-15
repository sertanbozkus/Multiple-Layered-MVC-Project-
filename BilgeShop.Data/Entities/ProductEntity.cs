using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }

        // null olmayan bir değer için (örnek -> decimal), configuration ile (required(false)) verdiğiniz zaman tanımlama ? koymak zorundasınız.

        // null olabilen bir değer için configuration ile required(false) verilirse yeterlidir. ? kullanmaya gerek yok. (örnek -> string / description)

        public int UnitsInStock { get; set; }
        public string ImagePath { get; set; }

        public int CategoryId { get; set; }

        // Relational Property

        public CategoryEntity Category { get; set; }

    }

    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(50);

            builder.Property(x => x.Description)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.UnitPrice)
                .IsRequired(false);

            builder.Property(x => x.ImagePath)
                .IsRequired(false);

            base.Configure(builder);
        }
    }
}
