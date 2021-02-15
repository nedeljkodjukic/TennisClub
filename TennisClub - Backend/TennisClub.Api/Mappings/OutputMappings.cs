using AutoMapper;
using System.Linq;
using System.Text;
using TennisClub.Api.Models.Input;
using TennisClub.Api.Models.Mongo;
using TennisClub.Api.Models.Output;

namespace TennisClub.Api.Mappings
{
    public class OutputMappings : Profile
    {
        public OutputMappings()
        {
            CreateMap<Player, PlayerOutputModel>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(source => Encoding.Unicode.GetString(source.Picture)))
                .ForMember(dest => dest.StandingPicture, opt => opt.MapFrom(source => Encoding.Unicode.GetString(source.StandingPicture)))
                .ForMember(dest => dest.Instagram, opt =>
                    opt.MapFrom(source => source.Accounts.SingleOrDefault(x => x.SocialNetwork == "Instagram").Link))
                .ForMember(dest => dest.Twitter,
                    opt => opt.MapFrom(source => source.Accounts.SingleOrDefault(x => x.SocialNetwork == "Twitter").Link));

            CreateMap<Rank, RankOutputModel>()
                .ForMember(dest => dest.PreviousRank, opt => opt.MapFrom(source => source.PreviousRankNumber))
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(source => source.RankNumber));

            CreateMap<Court, CourtInputOutputModel>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(source => Encoding.Unicode.GetString(source.Picture)));

            CreateMap<Tournament, TournamentOutputModel>()
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(source => Encoding.Unicode.GetString(source.Logo)));

            CreateMap<Match, MatchOutputModel>();

        }
    }
}
