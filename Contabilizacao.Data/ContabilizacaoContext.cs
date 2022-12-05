using Contabilizacao.Data.Mappings;
using Contabilizacao.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Contabilizacao.Data
{
    public class ContabilizacaoContext: DbContext
    {
        string _connectionString;
        public ContabilizacaoContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SupermarketMapping());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.Parse("8.0.31"));
             
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Supermarket> Supermarkets => Set<Supermarket>();
    }
}