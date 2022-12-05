using Contabilizacao.Domain.Requests;
using Contabilizacao.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Contabilizacao.API.Controllers
{
    [ApiController]
    [Route("/api/Products")]
    public class ProductController : ControllerBase
    {
        IProductAddingService _service;
        public ProductController(IProductAddingService service)
        {
            _service = service;
        }

        [HttpPost("Add")]
        public IActionResult AddToProduct([FromBody] AddRequest request)
        {
            _service.AddToProduct(request);
            return Ok();
        }

        [HttpPost("Register")]
        public IActionResult RegisterProduct([FromBody] RegisterRequest request)
        {
            _service.RegisterProduct(request);
            return Ok();
        }
    }
}
