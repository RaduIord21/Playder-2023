using System;
using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
    public class UserCredentialRepository : GenericRepository<UserCredential>, IUserCredentialRepository
    {
        public UserCredentialRepository(SummerCampDbContext context) : base(context)
        {
        }
    }
}

