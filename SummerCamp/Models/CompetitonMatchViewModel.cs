using System;
using System.ComponentModel.DataAnnotations;
using SummerCamp.DataModels.Models;

namespace SummerCamp.Models
{
	public class CompetitonMatchViewModel
	{
        public int? Id { get; set; }

        public int? CompetitionId { get; set; }

        [Required(ErrorMessage = "Va rugam selectati o echipa")]
        public int? HomeTeamId { get; set; }

        [Required(ErrorMessage = "Va rugam selectati o echipa")]
        public int? AwayTeamId { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati numarul de goluri")]
        public int? HomeTeamGoals { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati numarul de goluri")]
        public int? AwayTeamGoals { get; set; }

        public virtual Team? AwayTeam { get; set; } = null!;

        public virtual Competition? Competition { get; set; } = null!;

        public virtual Team? HomeTeam { get; set; } = null!;
    }
}

