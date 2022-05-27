using System.ComponentModel.DataAnnotations;

namespace SoftwareMind.Core.Helpers;

public class User
{
    [Key] public string Username { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }
}