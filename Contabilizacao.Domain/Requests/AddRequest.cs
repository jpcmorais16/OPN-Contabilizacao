using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Domain.Requests
{
    public class AddRequest
    {
        public string Code { get; set; }
        public int Shift { get; set; }
        public int Amount { get; set; }
        public string SupermarketName { get; set; }
        public bool IsWeighted { get; set; }
    }
}
