using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Services
{
    public class PlayerService : BaseMongoRepository
    {
        private readonly IMapper mapper;

        public PlayerService(IConfiguration configuration, IMapper mapper)
            : base(configuration)
        {
            this.mapper = mapper;
        }

        public async Task<PlayerOutputModel> Get(string id)
        {
            var ranksCollection = this.db.GetCollection<Rank>("Ranks");

            var matchesCollection = this.db.GetCollection<Match>("Matches");

            var player = await this.Get<Player>("Players", id)
                .SingleOrDefaultAsync();

            if (player == null)
            {
                throw new Exception("Player not found");
            }

            var rank = await ranksCollection.AsQueryable()
                .Where(rank => rank.PlayerId == id)
                .OrderByDescending(rank => rank.Year).OrderByDescending(rank => rank.Week)
                .FirstOrDefaultAsync();

            var matches = await matchesCollection.AsQueryable()
                .Where(x => x.FirstPlayerId == id || x.SecondPlayerId == id)
                .Select(x => new
                {
                    x.FirstPlayerId,
                    x.SecondPlayerId,
                    x.WinnerId
                })
                .ToListAsync();


            return this.mapper.Map<Player, PlayerOutputModel>(player, opt => opt.AfterMap((a, b) =>
            {
                b.Rank = rank != null ? rank.RankNumber : 0;
                b.TotalWins = matches.Where(x => x.WinnerId == id).Count();
                b.TotalLosses = matches.Count() - b.TotalWins;
            }));
        }

        public async Task<IEnumerable<PlayerOutputModel>> GetAll()
        {
            var players = await this.GetAll<Player>("Players").ToListAsync();

            return this.mapper.Map<IEnumerable<PlayerOutputModel>>(players);
        }


        public async Task<IEnumerable<PlayerBasicOutputModel>> GetPlayersBasic()
        {

            var playersCollection = this.db.GetCollection<Player>("Players");

            var players = await playersCollection.AsQueryable()
                .Select(player => new PlayerBasicOutputModel
                {
                    Id = player.Id,
                    FirstName = player.FirstName,
                    LastName = player.LastName
                })
                .ToListAsync();

            return players;
        }

        public async Task<IEnumerable<MatchOutputModel>> GetFinalMatches(string id)
        {
            return await this.GetMatchesBase(m => (m.FirstPlayerId == id || m.SecondPlayerId == id) && m.Phase == "Final");
        }

        public async Task<IEnumerable<MatchOutputModel>> GetRecentMaches(string id, int count)
        {
            return await this.GetMatchesBase(m => m.FirstPlayerId == id || m.SecondPlayerId == id);
        }

        public async Task<HeadToHeadOutputModel> GetHeadToHead(string firstPlayerId, string secondPlayerId)
        {

            var playersCollection = this.db.GetCollection<Player>("Players");

            var players = (await playersCollection.AsQueryable()
                .Where(x => x.Id == firstPlayerId || x.Id == secondPlayerId)
                .Select(x => new { x.Id, x.FirstName, x.LastName })
                .ToListAsync())
                .ToDictionary(x => x.Id, x => x.FirstName + " " + x.LastName);

            var matchesCollection = this.db.GetCollection<Match>("Matches");
            var tournametsCollection = this.db.GetCollection<Tournament>("Tournaments");

            var matches = await (from m in matchesCollection.AsQueryable()
                                 where (m.FirstPlayerId == firstPlayerId && m.SecondPlayerId == secondPlayerId)
                                 || (m.FirstPlayerId == secondPlayerId && m.SecondPlayerId == firstPlayerId)
                                 join t in tournametsCollection on m.TournamentId equals t.Id
                                 select new
                                 {
                                     Match = m,
                                     Tournament = t
                                 }
                             ).ToListAsync();


            var groupedBySurface = matches.GroupBy(x => x.Tournament.Surface);

            var modelToReturn = new HeadToHeadOutputModel()
            {
                FirstPlayerWins = groupedBySurface.ToDictionary(x => x.Key, x => x.Count(y => y.Match.WinnerId == firstPlayerId)),
                SecondPlayerWins = groupedBySurface.ToDictionary(x => x.Key, x => x.Count(y => y.Match.WinnerId == secondPlayerId)),
                FirstPlayerGrandSlamWins = matches.Where(x => x.Tournament.Category == "Grand Slam" && x.Match.WinnerId == firstPlayerId).Count(),
                SecondPlayerGrandSlamWins = matches.Where(x => x.Tournament.Category == "Grand Slam" && x.Match.WinnerId == secondPlayerId).Count(),
                RecentMatches = this.MapMatchesIntoOutputModels(matches, players)
            };

            return modelToReturn;
        }

        public async Task<string> Insert(PlayerInputModel input)
        {
            var player = this.mapper.Map<Player>(input);

            return await this.Insert<Player>("Players", player);
        }

        public async Task Update(string id, PlayerInputModel input)
        {
            var playerInput = this.mapper.Map<Player>(input);

            await this.Update<Player>("Players", id, playerInput);
        }

        public async Task Remove(string id)
        {
            await this.Remove<Player>("Players", id);
        }


        private async Task<IEnumerable<MatchOutputModel>> GetMatchesBase(Expression<Func<Match, bool>> predicate)
        {
            var playersCollection = this.db.GetCollection<Player>("Players");
            var matchesCollection = this.db.GetCollection<Match>("Matches");
            var tournametsCollection = this.db.GetCollection<Tournament>("Tournaments");


            var test = matchesCollection.AsQueryable().Where(predicate);

            var matches = await (from m in test
                                 join t in tournametsCollection on m.TournamentId equals t.Id
                                 join p1 in playersCollection on m.FirstPlayerId equals p1.Id
                                 join p2 in playersCollection on m.SecondPlayerId equals p2.Id
                                 select new
                                 {
                                     Match = new
                                     {
                                         m.Attendance,
                                         m.CourtName,
                                         m.Date,
                                         m.Phase,
                                         m.Sets
                                     },
                                     Tournament = t.Name,
                                     FirstPlayer = p1.FirstName + " " + p1.LastName,
                                     SecondPlayer = p2.FirstName + " " + p2.LastName
                                 }
                             ).ToListAsync();

            return matches.Select(m => new MatchOutputModel
            {
                Attendance = m.Match.Attendance,
                CourtName = m.Match.CourtName,
                Date = m.Match.Date,
                FirstPlayerFullName = m.FirstPlayer,
                Phase = m.Match.Phase,
                SecondPlayerFullName = m.SecondPlayer,
                Sets = m.Match.Sets,
                TournamentName = m.Tournament
            }).ToList();
        }


        private IEnumerable<MatchOutputModel> MapMatchesIntoOutputModels(dynamic matches, Dictionary<string, string> players)
        {
            var outputList = new List<MatchOutputModel>();
            foreach (var match in matches)
            {
                var outputModel = this.mapper.Map<MatchOutputModel>(match.Match);
                outputModel.TournamentName = match.Tournament.Name;
                outputModel.FirstPlayerFullName = players[match.Match.FirstPlayerId];
                outputModel.SecondPlayerFullName = players[match.Match.SecondPlayerId];
                outputList.Add(outputModel);
            }

            return outputList;
        }
    }
}
