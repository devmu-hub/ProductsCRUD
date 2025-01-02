using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_PromotionService.Read;
using ProductsCRUD.Application.S_PromotionService.Write;
using ProductsCRUD.Application.S_PromotionTypeService.Read;
using ProductsCRUD.WebApi.HTTPModels.Requests;
using ProductsCRUD.WebApi.HTTPModels.Responses;

namespace ProductsCRUD.WebApi.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class PromotionController(IMapper mapper,
        IPromotionReadService promotionReadService,
        IPromotionWriteService promotionWriteService,
        IPromotionTypeReadService promotionTypeReadService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPromotionReadService _promotionReadService = promotionReadService;
        private readonly IPromotionWriteService _promotionWriteService = promotionWriteService;
        private readonly IPromotionTypeReadService _promotionTypeReadService = promotionTypeReadService;



        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(ListBaseResponse<PromotionResponse>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _promotionReadService.GetAll();

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new ListBaseResponse<PromotionResponse>
            {
                Data = _mapper.Map<IEnumerable<PromotionResponse>>(response.Data)
            });
        }


        [HttpGet]
        [Route("Get/{promotionId}")]
        [ProducesResponseType(typeof(BaseResponse<PromotionResponse>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Get([FromRoute] int promotionId)
        {
            var response = await _promotionReadService.Get(promotionId);

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<PromotionResponse>
            {
                Data = _mapper.Map<PromotionResponse>(response.Data)
            });
        }


        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Create([FromBody] PromotionRequest PromotionRequest)
        {
            var response = await _promotionWriteService.Create(_mapper.Map<PromotionInput>(PromotionRequest));

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
        public async Task<IActionResult> Update([FromBody] PromotionRequest PromotionRequest)
        {
            var response = await _promotionWriteService.Update(_mapper.Map<PromotionInput>(PromotionRequest));

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
        [Route("Delete/{promotionId}")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> Delete([FromRoute] int promotionId)
        {
            var response = await _promotionWriteService.Delete(promotionId);

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


        [HttpGet]
        [Route("GetPromotionTypes")]
        [ProducesResponseType(typeof(ListBaseResponse<PromotionTypeResponse>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        public async Task<IActionResult> GetPromotionTypes()
        {
            var response = await _promotionTypeReadService.GetAll();

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new ListBaseResponse<PromotionTypeResponse>
            {
                Data = _mapper.Map<IEnumerable<PromotionTypeResponse>>(response.Data)
            });
        }


    }
}
