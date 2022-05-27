using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftwareMind.Core.Dtos;

public class NewReservation
{
    
    [JsonIgnore]
    public Guid? LocationId { get; set; }
    
    [JsonIgnore]
    public Guid? DeskId { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
}