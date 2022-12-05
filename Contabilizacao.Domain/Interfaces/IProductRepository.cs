using Contabilizacao.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Domain.Interfaces
{
    public interface IProductRepository
    {
        public void UpdateSupermarket(Supermarket supermarket);
        //public Product GetProduct(string code);
        public Supermarket GetSupermarket(string supermarketName);
        public void RegisterProduct(RegisterRequest request);
        Supermarket AddSupermarket(string supermarketName);
        Product GetProduct(string code);
        List<Product> EditProduct(EditionRequest request);
    }
}
