using System;
using SummerCamp.DataModels.Models;

namespace SummerCamp.Models
{
	public class TeamSponsorViewModel
	{
        public int Id { get; set; }

        public int? TeamId { get; set; }

        public int? SponsorId { get; set; }

        public virtual Sponsor? Sponsor { get; set; }

        public virtual Team? Team { get; set; }
    }
}

