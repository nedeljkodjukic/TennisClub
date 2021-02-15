﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Output;
using TennisClub.Api.Services;

namespace TennisClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingsController : ControllerBase
    {
        private readonly RankingService rankingService;

        public RankingsController(RankingService rankingService)
        {
            this.rankingService = rankingService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("get_top_rated")]
        public async Task<ActionResult<RankOutputModel>> GetTopRated([FromQuery] int year, [FromQuery] int week, [FromQuery] int count)
        {
            var result = await this.rankingService.GetTopRatedPlayers(year, week, count);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("get_ranks_for_player/{id}")]
        public async Task<ActionResult<Dictionary<int, List<RankBasicOutputModel>>>> GetRanksForPlayer([FromRoute] string id)
        {
            var result = await this.rankingService.GetRanksForPlayer(id);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("weeks")]
        public async Task<ActionResult<Dictionary<int, ICollection<object>>>> GetYearsAndWeeks([FromQuery] int years)
        {
            var dict = new Dictionary<int, List<object>>();
            var currentYear = DateTime.Now.Year;

            for (int i = currentYear - years; i <= currentYear; i++)
            {
                var numberOfWeeks = ISOWeek.GetWeeksInYear(i);

                dict.Add(i, new List<object>());

                for (int j = 1; j <= numberOfWeeks; j++)
                {
                    if (i == DateTime.Now.Year && ISOWeek.GetWeekOfYear(DateTime.Now) - 1 < j)
                        break;

                    var weekString = ISOWeek.ToDateTime(i, j, DayOfWeek.Monday).ToString("dd.MM.yyyy")
                        + " - "
                        + ISOWeek.ToDateTime(i, j, DayOfWeek.Sunday).ToString("dd.MM.yyyy");
                    dict[i].Add(new { weekNum = j, week = weekString });
                }
            }

            return Ok(dict);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("insert")]
        public async Task<ActionResult> InsertRankings([FromQuery] int year, [FromQuery] int week, [FromBody] IEnumerable<RankInputModel> input)
        {
            await this.rankingService.InsertRanks(year, week, input);

            return Ok();
        }

    }
}
