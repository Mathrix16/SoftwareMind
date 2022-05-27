using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftwareMind.Core.Dtos;

public class UpdatedDesk
{
    [Required] public bool IsAvailable { get; set; }

    [JsonIgnore] public Guid? LocationId { get; set; }

    [JsonIgnore] public Guid? Id { get; set; }
}