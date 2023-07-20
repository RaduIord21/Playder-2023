using System.ComponentModel.DataAnnotations;
using SummerCamp.DataModels.Enums;
using SummerCamp.DataModels.Models;

namespace SummerCamp.Models
{
    public class PlayerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati un nume.\n")]

        public string? FullName { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati o data a nasterii.\n")]

        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati o adresa.\n")]

        public string? Adress { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati pozitia jucatiorului.\n")]

        public PositionEnum? Position { get; set; }

        public int? TeamId { get; set; }

        [Required(ErrorMessage = "Va rugam adaugati numarul de pe tricou.\n")]

        public int? ShirtNumber { get; set; }

        public virtual Team? Team { get; set; }
    }
}