using Contabilizacao.Data.GoogleSheets;
using Contabilizacao.Data.Interfaces;
using Contabilizacao.Domain;
using Contabilizacao.Domain.Interfaces;
using Contabilizacao.Domain.Requests;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Data
{
    public class ProductRepository: IProductRepository
    {
        private ContabilizacaoContext _context;
        private GoogleSheetsConnection _connection;
        //private 

        public ProductRepository(ContabilizacaoContext context, GoogleSheetsConnection connection)
        {
            _context = context;
            _connection = connection;
        }

        public Supermarket AddSupermarket(string supermarketName)
        {
            var supermarket = new Supermarket { Name = supermarketName };
            _context.Supermarkets.Add(supermarket);

            _connection.UpdateSpreadsheet();

            return supermarket;
        }

        public List<Product> EditProduct(EditionRequest request)
        {
            var products = _context.Products.Where(p => p.Code == request.Code).ToList();
            products.ForEach(p => { p.Name = request.Name; p.Price = request.Price; p.WeightOrVolume = request.Weight; }) ;

            _context.SaveChanges();

            _connection.UpdateSpreadsheet();

            return products;
        }

        public Product GetProduct(string code)
        {
            return _context.Products.FirstOrDefault(p => p.Code.Equals(code))!;
        }

        public Supermarket GetSupermarket(string supermarketName)
        {
            return _context.Supermarkets.FirstOrDefault(s => s.Name.Equals(supermarketName))!;
        }

        public void RegisterProduct(RegisterRequest request)
        {
            foreach (var supermarket in _context.Supermarkets)
            {
                supermarket.AddProduct(new Product
                {
                    Name = request.Name,
                    Code = request.Code,
                    WeightOrVolume = request.Weight,
                    Price = request.Price,
                    SupermarketId = supermarket.Id

                });
            }
            _context.SaveChanges();

            _connection.UpdateSpreadsheet();
        }

        public void UpdateSupermarket(Supermarket supermarket)
        {
            _context.Supermarkets.Update(supermarket);
            _context.SaveChanges();

            _connection.UpdateSpreadsheet();
        }
    }
}
