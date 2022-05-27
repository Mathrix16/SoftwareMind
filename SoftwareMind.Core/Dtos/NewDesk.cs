using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftwareMind.Core.Dtos;

public class NewDesk
{
    [Required] public string Name { get; set; }

    public bool IsAvailable { get; set; } = true;

    [JsonIgnore] public Guid? LocationId { get; set; }
}