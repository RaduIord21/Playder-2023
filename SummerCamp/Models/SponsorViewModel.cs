using System;
using System.ComponentModel.DataAnnotations;
using SummerCamp.DataModels.Models;

namespace SummerCamp.Models
{
	public class SponsorViewModel
	{
        public int Id { get; set; }

        [Required(ErrorMessage ="Va rugam adaugati un nume. \n")]

        public string? Name { get; set; }

    }
}