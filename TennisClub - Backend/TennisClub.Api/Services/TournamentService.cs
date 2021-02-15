using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Services
{
    public class TournamentService : BaseMongoRepository
    {
        private readonly IMapper mapper;

        public TournamentService(IConfiguration configuration, IMapper mapper)
            : base(configuration)
        {
            this.mapper = mapper;
        }


        public async Task<TournamentOutputModel> Get(string id)
        {
            var tournament = await this.Get<Tournament>("Tournaments", id)
                                       .SingleOrDefaultAsync();

            return mapper.Map<TournamentOutputModel>(tournament);
        }

        public async Task<IEnumerable<TournamentOutputModel>> GetAll()
        {
            var tournaments = await this.GetAll<Tournament>("Tournaments")
                                       .ToListAsync();

            return mapper.Map<IEnumerable<TournamentOutputModel>>(tournaments);
        }

        public async Task<IEnumerable<TournamentBasicOutputModel>> GetTournamentsBasic()
        {
            //var tournamentsCollection = this.db.GetCollection<Tournament>("Tournaments");

            var tournaments = await this.GetAll<Tournament>("Tournaments")
                .Select(tournament => new TournamentBasicOutputModel
                {
                    Id = tournament.Id,
                    Name = tournament.Name,
                    Category = tournament.Category
                })
                .ToListAsync();

            return tournaments;
        }

        public async Task<IEnumerable<TournamentWinnerOutputModel>> GetPastWinners(string tournamentId)
        {
            var matchesCollection = this.db.GetCollection<Match>("Matches");
            var tournamentsCollection = this.db.GetCollection<Tournament>("Tournaments");
            var playersCollection = this.db.GetCollection<Player>("Players");

            var winners = await (from m in matchesCollection.AsQueryable()
                                 where m.TournamentId == tournamentId && m.Phase == "Final"
                                 orderby m.Date descending
                                 join p in playersCollection on m.WinnerId equals p.Id
                                 select new TournamentWinnerOutputModel
                                 {
                                     Player = new PlayerBasicOutputModel
                                     {
                                         Id = m.WinnerId,
                                         FirstName = p.FirstName,
                                         LastName = p.LastName
                                     },
                                     Year = m.Date.Year
                                 }).ToListAsync();

            return winners;
        }

        public async Task<IEnumerable<MatchOutputModel>> GetMatches(string tournamentId, int year)
        {
            var matchesCollection = this.db.GetCollection<Match>("Matches");
            var tournamentsCollection = this.db.GetCollection<Tournament>("Tournaments");
            var playersCollection = this.db.GetCollection<Player>("Players");


            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year + 1, 1, 1);

            var matches = await (from m in matchesCollection.AsQueryable()
                                 where m.Date >= startDate && m.Date < endDate && m.TournamentId == tournamentId
                                 join t in tournamentsCollection on m.TournamentId equals t.Id
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
                                     t.Name,
                                     FirstPlayer = new PlayerBasicOutputModel
                                     {
                                         Id = p1.Id,
                                         FirstName = p1.FirstName,
                                         LastName = p1.LastName
                                     },
                                     SecondPlayer = new PlayerBasicOutputModel
                                     {
                                         Id = p2.Id,
                                         FirstName = p2.FirstName,
                                         LastName = p2.LastName
                                     }
                                 }).ToListAsync();

            return matches.Select(m => new MatchOutputModel
            {
                Attendance = m.Match.Attendance,
                CourtName = m.Match.CourtName,
                Date = m.Match.Date,
                FirstPlayerFullName = m.FirstPlayer.FirstName + " " + m.FirstPlayer.LastName,
                Phase = m.Match.Phase,
                SecondPlayerFullName = m.SecondPlayer.FirstName + " " + m.SecondPlayer.LastName,
                Sets = m.Match.Sets,
                TournamentName = m.Name
            }).ToList();

        }

        public async Task<IEnumerable<PlayerBasicOutputModel>> GetTournamentRecord(string tournamentId)
        {

            var matchesCollection = this.db.GetCollection<Match>("Matches");
            var tournamentsCollection = this.db.GetCollection<Tournament>("Tournaments");
            var playersCollection = this.db.GetCollection<Player>("Players");

            var champions = await (from m in matchesCollection.AsQueryable()
                                   where m.TournamentId == tournamentId && m.Phase == "Final"
                                   join p in playersCollection on m.WinnerId equals p.Id
                                   orderby m.Date descending
                                   select new
                                   {
                                       p.Id,
                                       p.FirstName,
                                       p.LastName
                                   }).ToListAsync();

            if (champions.Count > 0)
            {
                var mostRecentChampion = new PlayerBasicOutputModel
                {
                    Id = champions[0].Id,
                    FirstName = champions[0].FirstName,
                    LastName = champions[0].LastName
                };

                var mostTitles = champions.GroupBy(x => x)
                    .OrderByDescending(gr => gr.Count())
                    .First().Key;

                var mostTitledPlayer = new PlayerBasicOutputModel
                {
                    Id = mostTitles.Id,
                    FirstName = mostTitles.FirstName,
                    LastName = mostTitles.LastName
                };


                return new[] { mostTitledPlayer, mostRecentChampion };
            }


            return null;


        }

        public async Task<string> Insert(TournamentInputModel input)
        {
            var tournament = this.mapper.Map<Tournament>(input);

            return await this.Insert<Tournament>("Tournaments", tournament);
        }

        public async Task Update(string id, TournamentInputModel input)
        {
            var tournamentInput = this.mapper.Map<Tournament>(input);

            await this.Update<Tournament>("Tournaments", id, tournamentInput);
        }

        public async Task Remove(string id)
        {
            await this.Remove<Tournament>("Tournaments", id);
        }
    }
}
