using Contabilizacao.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Domain
{
    public class Product
    {
        public Product()
        {

        }

        public int Id { get; set; } 
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double WeightOrVolume { get; set; }
        public bool HasVolume { get; set; } 
        public int SupermarketId { get; set; }
        public int FirstShiftAmount { get; set; }
        public int SecondShiftAmount { get; set; }
        public int ThirdShiftAmount { get; set; }
        public int ForthShiftAmount { get; set; }

        public void AddToShift(int amount, int shift)
        {
            switch (shift)
            {
                case 1: FirstShiftAmount += amount;
                    break;
                case 2: SecondShiftAmount += amount;
                    break;
                case 3: ThirdShiftAmount += amount;
                    break;
                case 4: ForthShiftAmount += amount;
                    break;
                default: throw new ArgumentOutOfRangeException("Turno inválido!");
            }
        }       
    }
}
