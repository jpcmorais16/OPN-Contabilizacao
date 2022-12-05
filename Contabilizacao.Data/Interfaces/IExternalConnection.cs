using Contabilizacao.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Data.Interfaces
{
    public interface IExternalConnection
    {
        public List<Product> GetProducts();
        public void AddProduct(Product product);
        public void UpdateProduct(Product product);
        void AddProductToSupermarket(string name, Product product);
        public void AddProductToSupermarket(Product product, string supermarketName);
        public void UpdateProductToSupermarket(Product product, string supermarketName);
    }
}
