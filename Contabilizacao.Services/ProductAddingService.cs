using Contabilizacao.Domain;
using Contabilizacao.Domain.Interfaces;
using Contabilizacao.Domain.Requests;
using Contabilizacao.Services.Interfaces;

namespace Contabilizacao.Services
{
    public class ProductAddingService: IProductAddingService
    {
        private readonly IProductRepository _repository;
        public ProductAddingService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Product CheckProduct(string code)
        {
            return _repository.GetProduct(code);
        }

        public List<Product> EditProduct(EditionRequest request)
        {
            return _repository.EditProduct(request);
        }

        public void RegisterProduct(RegisterRequest request)
        {
             _repository.RegisterProduct(request);
        }

        public void AddToProduct(AddRequest request)
        {
            var supermarket = _repository.GetSupermarket(request.SupermarketName);

            if (supermarket == null)
                supermarket = _repository.AddSupermarket(request.SupermarketName);

            supermarket.AddToProduct(request.Code, request.Amount, request.Shift);

            _repository.UpdateSupermarket(supermarket);
        }
    }
}
