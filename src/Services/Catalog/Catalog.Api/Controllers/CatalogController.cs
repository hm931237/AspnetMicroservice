using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{//
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger= logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>>  GetRoducts()
        {
            var products=_productRepository.GetProducts();
            return Ok(await products);
        }
        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if(product == null) 
                return NotFound();
            return Ok(product);
        }
        [Route("[action]/{category}",Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductByCategory(string category)
        {
            var product = await _productRepository.GetProductByCategory(category);
            return Ok(product);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _productRepository.CreateProduct(product);
            return  CreatedAtRoute("GetProduct",new {id=product.Id},product);
        }
        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> Update([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }
    }
}
