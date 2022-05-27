using System.ComponentModel.DataAnnotations;

namespace SoftwareMind.Core.Dtos;

public class NewLocation 
{
    [Required]
    public string Name { get; set; }
}