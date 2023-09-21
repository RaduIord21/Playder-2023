using System;
using System.ComponentModel.DataAnnotations;

namespace SummerCamp.Models
{
	public class UserCredentialViewModel
	{

        [Required(ErrorMessage = "Va rugam adaugati un nume de utilizator.\n")]

        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Va rugam adaugati o parola.\n")]

        public string PasswordHash { get; set; } = null!;
    }
}

