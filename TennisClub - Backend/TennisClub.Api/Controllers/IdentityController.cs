using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TennisClub.Api.Enums;
using TennisClub.Api.Models.Identity;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService identityService;

        public IdentityController(IdentityService identityService)
        {
            this.identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenOutputModel>> Login([FromBody] LoginInputModel input)
        {
            var token = await this.identityService.LoginUserAsync(input);

            this.Response.Cookies.Delete(".AspNetCore.Identity.Application");

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register/{role}")]
        public async Task<ActionResult<int>> Register([FromRoute] Role role, [FromBody] RegisterInputModel input)
        {
            var user = await this.identityService.RegisterUserAsync(input);

            await this.identityService.RegisterUserToRoleAsync(user, role);

            return Ok(user.Id);
        }
    }
}
