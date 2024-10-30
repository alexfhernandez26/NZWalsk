using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalskApi.Models.DTO;

namespace NZWalskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolManagerController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolManagerController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleDto.RoleName);

            if (!roleExist) 
            {
                var role = new IdentityRole
                {
                    Name = roleDto.RoleName,
                    NormalizedName = roleDto.RoleName.ToUpper(), 
                    ConcurrencyStamp = Guid.NewGuid().ToString() 
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded) 
                {
                    return Ok($"Role '{roleDto.RoleName}' created successfuly");
                }
                return BadRequest("Failed to create new role");
            }

            return BadRequest("Role already exist");
        }

        [HttpPost]
        [Route("AddRoleToUser")]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> AddRoleToUser(UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDto.UserName);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var roleExist = await _roleManager.FindByNameAsync(userRoleDto.RoleName);
            
            if (roleExist == null)
            {
                return BadRequest("Role not found");
            }

            var result = await _userManager.AddToRoleAsync(user,userRoleDto.RoleName);
            
            if(result.Succeeded)
            {
                return Ok($"Role '{userRoleDto.RoleName}' add to user {userRoleDto.UserName}");
            }

            return BadRequest("Failed to add roles");
        }

        [HttpDelete]
        [Route("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserRoleDto userRoleDto)
        {
            var user = await _userManager.FindByEmailAsync(userRoleDto.UserName);
            if(user == null)
            {
                return BadRequest("User Not Found");
            }

            var roleExist = await _roleManager.FindByNameAsync(userRoleDto.RoleName);
            if (roleExist == null)
            {
                return BadRequest("Role Not Found");
            }

            var result = await _userManager.RemoveFromRoleAsync(user,userRoleDto.RoleName);

            if(result.Succeeded)
            {
                return Ok($"Role '{userRoleDto.RoleName}' removed from user '{userRoleDto.UserName}'.");
            }

            return BadRequest("Failed to remove role");
        }
    }
}
