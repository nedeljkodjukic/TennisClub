using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService playerService;

        public PlayerController(PlayerService playerService)
        {
            this.playerService = playerService;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<PlayerOutputModel>> Get([FromRoute] string id)
        {
            var result = await this.playerService.Get(id);

            return Ok(result);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        [Route("get_all_basic")]
        public async Task<ActionResult<IEnumerable<PlayerBasicOutputModel>>> GetAllBasic()
        {
            var result = await this.playerService.GetPlayersBasic();

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("get_head_to_head")]
        public async Task<ActionResult<HeadToHeadOutputModel>> GetHeadToHead([FromQuery] string firstPlayerId, [FromQuery] string secondPlayerId)
        {
            var result = await this.playerService.GetHeadToHead(firstPlayerId, secondPlayerId);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("get_recent_matches/{id}")]
        public async Task<ActionResult<HeadToHeadOutputModel>> GetRecentMatches([FromRoute] string id)
        {
            var result = await this.playerService.GetRecentMaches(id, 5);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("get_title_matches/{id}")]
        public async Task<ActionResult<HeadToHeadOutputModel>> GetTitleMatches([FromRoute] string id)
        {
            var result = await this.playerService.GetFinalMatches(id);

            return Ok(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("get_all")]
        public async Task<ActionResult<IEnumerable<PlayerOutputModel>>> GetAll()
        {
            var result = await this.playerService.GetAll();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult<string>> Insert([FromBody] PlayerInputModel input)
        {
            var result = await this.playerService.Insert(input);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("update/{id}")]
        public async Task<ActionResult<string>> Update([FromRoute] string id, [FromBody] PlayerInputModel input)
        {
            await this.playerService.Update(id, input);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] string id)
        {
            await this.playerService.Remove(id);

            return Ok();
        }
    }
}
