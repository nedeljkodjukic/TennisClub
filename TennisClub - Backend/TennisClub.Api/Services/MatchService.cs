using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Services
{
    public class MatchService : BaseMongoRepository
    {
        private readonly IMapper mapper;

        public MatchService(IConfiguration configuration, IMapper mapper)
            : base(configuration)
        {
            this.mapper = mapper;
        }


        public async Task<string> Insert(MatchInputModel input)
        {
            var match = mapper.Map<Match>(input);

            var result = await this.Insert<Match>("Matches", match);


            var playersCollection = this.db.GetCollection<Player>("Players");

            await UpdatePlayerTitlesAfterMatch(playersCollection, input);

            return result;
        }

        private async Task UpdatePlayerTitlesAfterMatch(IMongoCollection<Player> collection, MatchInputModel input)
        {
            if (input.Phase != "Final")
            {
                return;
            }

            var winnerId = input.Sets.Count(set => set.FirstPlayerScore > set.SecondPlayerScore) >
                            input.Sets.Count(set => set.SecondPlayerScore > set.FirstPlayerScore) ?
                            input.FirstPlayerId : input.SecondPlayerId;

            var filterDef = Builders<Player>.Filter.Eq(player => player.Id, winnerId);
            var updateDef = Builders<Player>.Update.Inc(player => player.Titles, 1);

            var updateResult = await collection.UpdateManyAsync(filterDef, updateDef);
        }

        public async Task Update(string id, MatchUpdateInputModel input)
        {
            var matchInput = this.mapper.Map<Match>(input);
            await this.Update<Match>("Matches", id, matchInput);
        }

        public async Task Remove(string id)
        {
            await this.Remove<Match>("Matches", id);
        }
    }
}
