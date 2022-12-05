using Contabilizacao.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Data.Mappings
{
    public class SupermarketMapping : IEntityTypeConfiguration<Supermarket>
    {
        public void Configure(EntityTypeBuilder<Supermarket> builder)
        {
            builder.HasMany(s => s.ProductList).WithOne().HasForeignKey(p => p.SupermarketId);
        }
    }
}
