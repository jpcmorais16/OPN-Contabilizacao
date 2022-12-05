using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Domain.Requests
{
    public class RegisterRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public bool HasVolume { get; set; }
    }
}
