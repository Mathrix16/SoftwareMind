namespace SoftwareMind.Dal.Entities;

public class Desk
{
    public Guid Id { get; set; }
    
    public Guid LocationId { get; set; }
    
    public string Name { get; set; }
    
    public bool IsAvailable { get; set; }
}