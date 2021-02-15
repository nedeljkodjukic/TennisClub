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
    public class TournamentController : ControllerBase
    {
        private readonly TournamentService tournamentService;

        public TournamentController(TournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("get/{id}")]
        public async Task<ActionResult<TournamentOutputModel>> Get([FromRoute] string id)
        {
            var result = await this.tournamentService.Get(id);

            return Ok(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("get_all")]
        public async Task<ActionResult<TournamentOutputModel>> GetAll()
        {
            var result = await this.tournamentService.GetAll();

            return Ok(result);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("get_all_basic")]
        public async Task<ActionResult<IEnumerable<TournamentBasicOutputModel>>> GetAllBasic()
        {
            var result = await this.tournamentService.GetTournamentsBasic();

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("get_past_winners/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<TournamentWinnerOutputModel>>> GetPastWinners([FromRoute] string tournamentId)
        {
            var result = await this.tournamentService.GetPastWinners(tournamentId);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("get_records/{tournamentId}")]
        public async Task<ActionResult<IEnumerable<PlayerBasicOutputModel>>> GetTournamentRecord([FromRoute] string tournamentId)
        {
            var result = await this.tournamentService.GetTournamentRecord(tournamentId);

            return Ok(result);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        [Route("get_matches/{tournamentId}/{year}")]
        public async Task<ActionResult<MatchOutputModel>> GetMatches([FromRoute] string tournamentId, [FromRoute] int year)
        {
            var result = await this.tournamentService.GetMatches(tournamentId, year);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult<string>> Insert([FromBody] TournamentInputModel input)
        {
            var result = await this.tournamentService.Insert(input);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> Update([FromRoute] string id, [FromBody] TournamentInputModel input)
        {
            await this.tournamentService.Update(id, input);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] string id)
        {
            await this.tournamentService.Remove(id);

            return Ok();
        }
    }
}
