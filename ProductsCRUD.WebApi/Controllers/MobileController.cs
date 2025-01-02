using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_ProductService.Read;
using ProductsCRUD.WebApi.HTTPModels.Requests;
using ProductsCRUD.WebApi.HTTPModels.Responses;

namespace ProductsCRUD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileController(IMapper mapper,
        IProductReadService productReadService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProductReadService _productReadService = productReadService;



        [HttpGet]
        [Route("GetProducts")]
        [ProducesResponseType(typeof(BaseResponse<PagedBaseResponse<MobileProductResponse>>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> GetProducts([FromBody] MobileProductSearchRequest mobileProductSearchRequest)
        {
            MobileProductSearchInput mobileProductSearchInput = _mapper.Map<MobileProductSearchInput>(mobileProductSearchRequest);

            var response = await _productReadService.GetProductsForMobile(mobileProductSearchInput);

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<PagedBaseResponse<MobileProductResponse>>
            {
                Data = new PagedBaseResponse<MobileProductResponse>
                {
                    List = _mapper.Map<IEnumerable<MobileProductResponse>>(response.Data),
                    Count = response.Count
                }
            });
        }



    }
}
