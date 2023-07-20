using System;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(SummerCampDbContext context) : base(context)
        {
        }
    }
}

