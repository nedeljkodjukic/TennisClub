using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService matchService;

        public MatchController(MatchService matchService)
        {
            this.matchService = matchService;
        }

        [AllowAnonymous]
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult<string>> Insert([FromBody] MatchInputModel input)
        {
            var result = await this.matchService.Insert(input);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] MatchUpdateInputModel input)
        {
            await this.matchService.Update(id, input);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] string id)
        {
            await this.matchService.Remove(id);

            return Ok();
        }
    }
}
