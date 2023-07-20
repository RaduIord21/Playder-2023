using System.ComponentModel.DataAnnotations;
using SummerCamp.DataModels.Models;

namespace SummerCamp.Models
{
	public class TeamViewModel
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati o porecla.")]

        public string? NickName { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati un nume.")]

        public string? Name { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati un Antrenor.")]

        public int? CoachId { get; set; }

        public virtual Coach? Coach { get; set; }

        public List<int>? SelectedPlayerIds { get; set; }

        public List<int>? SelectedSponsorIds { get; set; }

        public virtual List<PlayerViewModel>? Players { get; set; } = new List<PlayerViewModel>();

        public virtual List<TeamSponsorViewModel>? TeamSponsors { get; set; } = new List<TeamSponsorViewModel>(); 

    }
}

