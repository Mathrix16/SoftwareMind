using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Core.Dtos;

public class ReservationDto
{
    
    public Guid Id { get; set; }
    
    public Guid LocationId { get; set; }
    
    public Guid DeskId { get; set; }
    
    public string Surname { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    public ReservationDto(Reservation reservation)
    {
        Id = reservation.Id;
        LocationId = reservation.LocationId;
        DeskId = reservation.DeskId;
        Surname = reservation.Surname;
        StartDate = reservation.StartDate;
        EndDate = reservation.EndDate;
    }
}