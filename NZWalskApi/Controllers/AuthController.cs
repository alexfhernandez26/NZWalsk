using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalskApi.Models.DTO;
using NZWalskApi.Repositories;

namespace NZWalskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenService;
        //private readonly SignInManager<IdentityUser> _signInManager;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
          //  _signInManager = signInManager;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var result =await _userManager.CreateAsync(IdentityUser,registerRequestDto.Password);

            if (result.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    result = await _userManager.AddToRolesAsync(IdentityUser, registerRequestDto.Roles);

                    if (result.Succeeded) 
                    {
                        return Ok("User Was Register, please login");
                    }
                }
            }

            return BadRequest("Something wen wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.UserName);

            if(user != null)
            {
                var checkPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordCorrect)
                {
                    //Create token
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null) 
                    {
                        var jwt =  _tokenService.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwt,
                        };

                        return Ok(response);
                    }
                }
            }
            return BadRequest("user or email was incorrect");
        }
    }
}
