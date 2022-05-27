using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Core.Dtos;

public class DeskDto
{
    public Guid Id { get; set; }
    
    public Guid LocationId { get; set; }
    
    public string Name { get; set; }
    
    public bool IsAvailable { get; set; }

    public DeskDto(Desk desk)
    {
        Id = desk.Id;
        LocationId = desk.LocationId;
        Name = desk.Name;
        IsAvailable = desk.IsAvailable;
    }
}