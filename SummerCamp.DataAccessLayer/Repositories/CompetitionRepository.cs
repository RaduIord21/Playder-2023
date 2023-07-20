using System;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
    public class CompetitionRepository : GenericRepository<Competition>, ICompetitionRepository
    {
        public CompetitionRepository(SummerCampDbContext context) : base(context)
        {
        }
    }
}

