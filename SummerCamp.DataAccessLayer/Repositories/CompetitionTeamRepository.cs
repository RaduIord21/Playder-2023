using System;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
    public class CompetitionTeamRepository : GenericRepository<CompetitonTeam>, ICompetitionTeamRepository
    {
        public CompetitionTeamRepository(SummerCampDbContext context) : base(context)
        {
        }
    }
}

