using Contabilizacao.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Domain
{
    public class Supermarket
    {
        public Supermarket()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<int> AmountPerShift { get; set; }
        public List<Product> ProductList { get; set; }
        
        public void AddToProduct(string code, int amount, int shift)
        {
            var product = ProductList.FirstOrDefault(p => p.Code.Equals(code));
            product.AddToShift(amount, shift);
        }

        public void AddProduct(Product product)
        {
            ProductList.Add(product);
        }
    }
}
