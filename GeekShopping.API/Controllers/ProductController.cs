using GeekShopping.API.Data.ValueObjects;
using GeekShopping.API.Repository.Interfaces;
using GeekShopping.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController(IProductRepository repository, ILogger<ProductController> logger) : ControllerBase
    {
        private readonly IProductRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<ProductController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> Get()
        {
            _logger.LogInformation("Fetching all products");

            var products = await _repository.FindAll();
            return Ok(products);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductVO>> Get(long id)
        {
            _logger.LogInformation("Fetching product with ID: {id}", id);

            var product = await _repository.FindById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductVO>> Post([FromBody] ProductVO product)
        {
            if (product == null)
            { 
                _logger.LogWarning("Attempted to create a null product");
                return BadRequest(); 
            }

            var createdProduct = await _repository.Create(product);
            
            _logger.LogInformation("Product created with ID: {id}", createdProduct.Id);

            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<ProductVO>> Put([FromBody] ProductVO product)
        {
            if (product == null)
            {
                _logger.LogWarning("Attempted to create a null product");
                return BadRequest();
            }

            var updatedProduct = await _repository.Update(product);

            _logger.LogInformation("Product updated with ID: {id}", updatedProduct.Id);

            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Delete(long id)
        {
            _logger.LogInformation("Attempting to delete product with ID: {id}", id);
            
            var deleted = await _repository.Delete(id);
            
            if (!deleted)
            {
                _logger.LogWarning("Product with ID: {id} not found for deletion", id);
                return NotFound();
            }

            _logger.LogInformation("Product with ID: {id} deleted successfully", id);

            return Ok(true);
        }
    }
}
