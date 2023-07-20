using System;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
    public class TeamSponsorsRepository : GenericRepository<TeamSponsor>, ITeamSponsorRepository
    {
        public TeamSponsorsRepository(SummerCampDbContext context) : base(context)
        {
        }
    }
}

