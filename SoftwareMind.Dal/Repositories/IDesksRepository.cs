using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal.Repositories;

public interface IDesksRepository
{
    Task<IEnumerable<Desk>> GetDesksAsync(Guid locationId, CancellationToken cancellationToken);

    Task<Desk?> GetDeskById(Guid locationId, Guid deskId, CancellationToken cancellationToken);

    Task<Guid> AddDesk(Desk desk, CancellationToken cancellationToken);

    Task DeleteDesk(Guid locationId, Guid id, CancellationToken cancellationToken);

    Task UpdateDesk(Desk inputDesk, CancellationToken cancellationToken);
    
    Task<bool> DeskExists(Guid locationId, Guid id, CancellationToken cancellationToken);

    Task<bool> DeskHasAnyActiveReservations(Guid locationId, Guid id, CancellationToken cancellationToken);
}