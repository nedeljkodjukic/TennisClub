using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Services
{
    public class RankingService : BaseMongoRepository
    {
        private readonly IMapper mapper;

        public RankingService(IConfiguration configuration, IMapper mapper)
            : base(configuration)
        {
            this.mapper = mapper;
        }

        public async Task<Dictionary<int,List<RankBasicOutputModel>>> GetRanksForPlayer(string playerId)
        {
            //var ranksCollection = this.db.GetCollection<Rank>("Ranks");

            var ranks = (await this.GetAll<Rank>("Ranks")
                .Where(rank => rank.PlayerId == playerId)
                .Select(rank => new
                {
                    rank.Year,
                    rank.Week,
                    rank.WeekDuration,
                    rank.RankNumber,
                    rank.Points
                })
                .ToListAsync())
                .GroupBy(rank => rank.Year)
                .ToDictionary(g => g.Key, g => g.Select(x => new RankBasicOutputModel
                {
                    Points = x.Points,
                    RankNumber = x.RankNumber,
                    Week = x.Week,
                    WeekDuration = x.WeekDuration
                }).ToList());

            return ranks;
        }

        public async Task InsertRanks(int year, int week, IEnumerable<RankInputModel> input)
        {
            var ranksCollection = this.db.GetCollection<Rank>("Ranks");

            var t = ISOWeek.GetWeeksInYear(year - 1);

            Expression<Func<Rank, bool>> filter;

            if (week == 1)
            {
                filter = rank => rank.Year == year - 1 && rank.Week == t;
            }
            else
            {
                filter = rank => rank.Year == year && rank.Week == week - 1;
            }

            var previousRanks = (await ranksCollection.AsQueryable()
                .Where(filter)
                .Select(rank => new { rank.PlayerId, rank.RankNumber })
                .ToListAsync())
                .ToDictionary(rank => rank.PlayerId, rank => rank.RankNumber);


            var ranks = mapper.Map<IEnumerable<Rank>>(input).ToList();
            ranks.ForEach(rank =>
            {
                rank.Year = year;
                rank.Week = week;
                rank.WeekDuration =
                ISOWeek.ToDateTime(year, week, DayOfWeek.Monday).ToString("dd.MM.yyyy")
                + " - "
                + ISOWeek.ToDateTime(year, week, DayOfWeek.Sunday).ToString("dd.MM.yyyy");
                rank.PreviousRankNumber = previousRanks.ContainsKey(rank.PlayerId) ? previousRanks[rank.PlayerId] : 0;
            });

            await ranksCollection.InsertManyAsync(ranks);
        }

        public async Task<IEnumerable<RankOutputModel>> GetTopRatedPlayers(int year, int week, int topCount)
        {
            if (year == 0 || week == 0)
            {
                var currentDate = DateTime.Now.AddDays(-7);
                year = ISOWeek.GetYear(currentDate);
                week = ISOWeek.GetWeekOfYear(currentDate);
            }

            var ranksCollection = this.db.GetCollection<Rank>("Ranks");
            var playersCollection = this.db.GetCollection<Player>("Players");

            var ranks = (await ranksCollection.AsQueryable()
                .Where(rank => rank.Year == year && rank.Week == week)
                .OrderBy(rank => rank.RankNumber)
                .Take(topCount)
                .Join(playersCollection,
                rank => rank.PlayerId,
                player => player.Id,
                (rank, player) => new
                {
                    Picture = player.Picture,
                    Model = new RankOutputModel
                    {
                        FirstName = player.FirstName,
                        LastName = player.LastName,
                        PlayerId = player.Id,
                        Points = rank.Points,
                        Country = player.Country,
                        PreviousRank = rank.PreviousRankNumber,
                        Rank = rank.RankNumber,
                    }
                })
                .ToListAsync());


            return ranks.Select(x => { x.Model.Picture = Encoding.Unicode.GetString(x.Picture); return x.Model; });
        }
    }
}
