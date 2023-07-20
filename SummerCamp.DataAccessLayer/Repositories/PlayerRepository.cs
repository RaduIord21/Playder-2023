using SummerCamp.DataAccessLayer.Interfaces;
using SummerCamp.DataModels.Models;

namespace SummerCamp.DataAccessLayer.Repositories
{
	public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
	{
		public PlayerRepository(SummerCampDbContext context) : base(context)
		{
		
		}
	}
}

