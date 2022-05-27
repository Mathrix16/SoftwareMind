namespace SoftwareMind.Dal.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    
    public Guid LocationId { get; set; }
    
    public Guid DeskId { get; set; }
    
    public string Surname { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
}