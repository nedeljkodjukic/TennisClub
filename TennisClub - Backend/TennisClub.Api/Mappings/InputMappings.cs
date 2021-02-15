using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Mappings
{
    public class InputMappings : Profile
    {
        public InputMappings()
        {
            CreateMap<PlayerInputModel, Player>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(source => Encoding.Unicode.GetBytes(source.Picture)))
                .ForMember(dest => dest.StandingPicture, opt => opt.MapFrom(source => Encoding.Unicode.GetBytes(source.StandingPicture)));

            CreateMap<TournamentInputModel, Tournament>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(source => Encoding.Unicode.GetBytes(source.Logo)));

            CreateMap<RankInputModel, Rank>();

            CreateMap<CourtInputOutputModel, Court>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(source => Encoding.Unicode.GetBytes(source.Picture)));

            CreateMap<MatchInputModel, Match>()
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(source => source.Sets.Select(set => set.Duration).Sum()))
                .ForMember(dest => dest.WinnerId,
                    opt => opt
                        .MapFrom(source =>
                            source.Sets.Count(set => set.FirstPlayerScore > set.SecondPlayerScore) >
                            source.Sets.Count(set => set.SecondPlayerScore > set.FirstPlayerScore) ?
                            source.FirstPlayerId : source.SecondPlayerId
                            )
                        );

            CreateMap<MatchUpdateInputModel, Match>()
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(source => source.Sets.Select(set => set.Duration).Sum()));
        }
    }
}
