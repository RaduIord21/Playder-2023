using System;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
    public class CompetitionMatchRepository : GenericRepository<CompetitionMatch>, ICompetitionMatchRepository
    {
        public CompetitionMatchRepository(SummerCampDbContext context) : base(context)
        {
        }
    }
}

