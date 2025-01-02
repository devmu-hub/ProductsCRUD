using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_ProductService.Read;
using ProductsCRUD.Application.S_ProductService.Write;
using ProductsCRUD.WebApi.HTTPModels.Requests;
using ProductsCRUD.WebApi.HTTPModels.Responses;

namespace ProductsCRUD.WebApi.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProductController(IMapper mapper,
        IProductWriteService productWriteService,
        IProductReadService productReadService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProductWriteService _productWriteService = productWriteService;
        private readonly IProductReadService _productReadService = productReadService;




        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(ListBaseResponse<ProductResponse>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productReadService.GetAll();

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new ListBaseResponse<ProductResponse>
            {
                Data = _mapper.Map<IEnumerable<ProductResponse>>(response.Data)
            });
        }


        [HttpGet]
        [Route("Get/{productId}")]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Get([FromRoute] int productId)
        {
            var response = await _productReadService.Get(productId);

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<ProductResponse>
            {
                Data = _mapper.Map<ProductResponse>(response.Data)
            });
        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Create([FromBody] ProductRequest productRequest)
        {
            var response = await _productWriteService.Create(_mapper.Map<ProductInput>(productRequest));

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<string>
            {
                Data = "The operation completed successfully"
            });
        }


        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Update([FromBody] ProductRequest productRequest)
        {
            var response = await _productWriteService.Update(_mapper.Map<ProductInput>(productRequest));

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<string>
            {
                Data = "The operation completed successfully"
            });
        }


        [HttpDelete]
        [Route("Delete/{productId}")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            var response = await _productWriteService.Delete(productId);

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<string>
            {
                Data = "The operation completed successfully"
            });
        }


        [HttpPost]
        [Route("AssignPromotion")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> AssignPromotion([FromBody] AssignPromotionRequest assignPromotionRequst)
        {
            var response = await _productWriteService.AssignPromotion(_mapper.Map<AssignPromotionInput>(assignPromotionRequst));

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<string>
            {
                Data = "The operation completed successfully"
            });
        }




    }
}
