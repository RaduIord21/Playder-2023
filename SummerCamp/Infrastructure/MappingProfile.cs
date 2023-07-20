using AutoMapper;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Infrastructure
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Coach, CoachViewModel>().ReverseMap();
            CreateMap<Player, PlayerViewModel>().ReverseMap();
            CreateMap<Team, TeamViewModel>().ReverseMap();
            CreateMap<Sponsor, SponsorViewModel>().ReverseMap();
            CreateMap<Competition, CompetitionViewModel>().ReverseMap();
            CreateMap<CompetitionMatch, CompetitonMatchViewModel>().ReverseMap();
            CreateMap<CompetitonTeam, CompetitionTeamViewModel>().ReverseMap();
        }
    }
}

