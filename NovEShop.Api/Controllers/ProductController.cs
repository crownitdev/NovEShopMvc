using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovEShop.Handler.Infrastructure;
using NovEShop.Handler.Products.Commands;
using NovEShop.Handler.Products.Queries;
using System.Threading.Tasks;

namespace NovEShop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IBroker _broker;

        public ProductController(IBroker broker)
        {
            _broker = broker;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsPagingQuery request)
        {
            var response = await _broker.Query(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByLanguagePaging([FromQuery] GetAllProductByLanguageQuery request)
        {
            var response = await _broker.Query(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByNamePaging([FromQuery] GetProductsByNameQuery request)
        {
            var response = await _broker.Query(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById([FromQuery] GetProductByIdQuery request)
        {
            var response = await _broker.Query(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsByCategoryPaging([FromQuery] GetProductsByCategoryNamePagingQuery request)
        {
            var response = await _broker.Query(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _broker.Command(request);
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(CreateProductCommandHandler), request, response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _broker.Command(request);
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteProductCommand request)
        {
            var response = await _broker.Command(request);
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPatch("/Price/{ProductId}/{NewPrice}")]
        public async Task<IActionResult> UpdateProductPrice([FromQuery] UpdateProductPriceCommand request)
        {
            var response = await _broker.Command(request);
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductStock([FromBody] UpdateProductStockCommand request)
        {
            var response = await _broker.Command(request);
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductViewCount([FromQuery] UpdateViewCountCommand request)
        {
            var response = await _broker.Command(request);
            if (response.IsSucceed == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{ProductId}/Images/{ImageId}")]
        public async Task<IActionResult> GetImageById(GetProductImageByIdQuery request)
        {
            var image = await _broker.Query(request);
            if (image == null)
                return BadRequest($"Không thể tìm thấy hình ảnh ${request.ImageId}");

            return Ok(image);
        }

        [HttpPost("{ProductId}/images")]
        public async Task<IActionResult> CreateImage(AddProductImageCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _broker.Command(request);
            if (response.Data == 0)
                return BadRequest();

            var image = await _broker.Query(new GetProductImageByIdQuery() { ImageId = response.Data});

            return CreatedAtAction(nameof(GetImageById), new { ImageId = response.Data }, image);
        }

        [HttpPut("{ProductId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage([FromForm] UpdateProductImageCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _broker.Command(request);
            if (result.Data == 0)
                return BadRequest($"Đã có lỗi xảy ra khi xoá hình ảnh ${request.ImageId}");

            return Ok(result);
        }

        [HttpDelete("{ProductId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> RemoveImage(DeleteProductImageCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _broker.Command(request);
            if (result.Data == 0)
                return BadRequest("Đã có lỗi xảy ra khi xoá hình ảnh");

            return Ok(result);
        }
    }
}
