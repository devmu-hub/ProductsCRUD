using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_AuthenticationService;
using ProductsCRUD.WebApi.HTTPModels.Requests;
using ProductsCRUD.WebApi.HTTPModels.Responses;
using ProductsCRUD.WebApi.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductsCRUD.WebApi.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AuthenticationController(IMapper mapper,
        IAuthenticationService authenticationService,
        IOptions<JwtTokenSettings> jwtTokenSettings) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly JwtTokenSettings _jwtTokenSettings = jwtTokenSettings.Value;



        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(BaseResponse<LoginResponse>), 200)]
        [ProducesResponseType(typeof(FailedResponse), 400)]
        [ProducesResponseType(typeof(FailedResponse), 500)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            LoginInput loginInput = _mapper.Map<LoginInput>(loginRequest);

            var response = await _authenticationService.Login(loginInput);

            if (response.IsExistException)
                return StatusCode(500, new FailedResponse
                {
                    Errors = "There Exist Something Wrong, try it again later"
                });

            if (!response.Success)
                return BadRequest(new FailedResponse { Errors = string.Join(" \n ", response.ErrorMessages) });

            return Ok(new BaseResponse<LoginResponse>
            {
                Data = new LoginResponse
                {
                    Token = GenerateToken(response.Data.Id)
                }
            });
        }















        private string GenerateToken(int id)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            byte[] key = Encoding.UTF8.GetBytes(_jwtTokenSettings.SigningKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _jwtTokenSettings.Issuer,
                Audience = _jwtTokenSettings.Audience,
                Subject = new ClaimsIdentity([
                    new("Id", id.ToString()),
                    new(ClaimTypes.Role, "Admin")
                ]),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.Now.AddHours(8)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}
