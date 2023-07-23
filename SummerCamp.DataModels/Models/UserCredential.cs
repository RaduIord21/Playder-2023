using System;
using System.Collections.Generic;

namespace SummerCamp.DataModels.Models;

public partial class UserCredential
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
