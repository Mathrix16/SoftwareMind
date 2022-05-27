using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Core.Dtos;

public class LocationDto
{
    public LocationDto(Location location)
    {
        Name = location.Name;
        Id = location.Id;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}