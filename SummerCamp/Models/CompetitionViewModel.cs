using System;
using System.ComponentModel.DataAnnotations;
using SummerCamp.DataModels.Models;

namespace SummerCamp.Models
{
	public class CompetitionViewModel
	{
        public int? Id { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati un nume.")]

        public string? Name { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati Un numar de echipe.")]

        public int? NumberOfTeams { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati o adresa.")]

        public string? Adress { get; set; }
        [Required(ErrorMessage = "Va rugam adaugati o data de start.")]

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? SponsorId { get; set; }

        public List<int>? SelectedTeamIds { get; set; }

        public virtual List<TeamViewModel>? Teams { get; set; } = new List<TeamViewModel>();
    }
}

