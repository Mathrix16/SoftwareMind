using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftwareMind.Core.Dtos;

public class UpdatedReservation
{
    [JsonIgnore] public Guid? OldLocationId { get; set; }

    [JsonIgnore] public Guid? OldDeskId { get; set; }

    [JsonIgnore] public Guid? Id { get; set; }

    [Required] public Guid NewLocationId { get; set; }

    [Required] public Guid NewDeskId { get; set; }

    [Required] public DateTime StartDate { get; set; }

    [Required] public DateTime EndDate { get; set; }
}