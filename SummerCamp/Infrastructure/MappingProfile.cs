using System.Text;
using AutoMapper;
using Org.BouncyCastle.Crypto.Digests;
using SummerCamp.DataModels.Models;
using SummerCamp.Models;

namespace SummerCamp.Infrastructure
{
	public class MappingProfile : Profile
	{
        private string HashPassword(string password)
        {
            Sha224Digest sha224 = new Sha224Digest();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            sha224.BlockUpdate(passwordBytes, 0, passwordBytes.Length);
            byte[] hashBytes = new byte[sha224.GetDigestSize()];
            sha224.DoFinal(hashBytes, 0);

            // Convert the hash bytes to a hexadecimal string representation
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        public MappingProfile()
        {
            CreateMap<Coach, CoachViewModel>().ReverseMap();
            CreateMap<Player, PlayerViewModel>().ReverseMap();
            CreateMap<Team, TeamViewModel>().ReverseMap();
            CreateMap<Sponsor, SponsorViewModel>().ReverseMap();
            CreateMap<Competition, CompetitionViewModel>().ReverseMap();
            CreateMap<CompetitionMatch, CompetitonMatchViewModel>().ReverseMap();
            CreateMap<CompetitonTeam, CompetitionTeamViewModel>().ReverseMap();
            CreateMap<UserCredential, UserCredentialViewModel>().ReverseMap();

        }
    }
}