using Contabilizacao.Domain;
using Contabilizacao.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilizacao.Services.Interfaces
{
    public interface IProductAddingService
    {
        public void RegisterProduct(RegisterRequest request);
        public void AddToProduct(AddRequest request);
    }
}
