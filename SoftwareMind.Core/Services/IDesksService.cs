using SoftwareMind.Core.Dtos;

namespace SoftwareMind.Core.Services;

public interface IDesksService
{
    Task<IEnumerable<DeskDto>> GetDesksAsync(Guid locationId, CancellationToken cancellationToken);

    Task<Guid> AddDesk(NewDesk newDesk, CancellationToken cancellationToken);

    Task DeleteDesk(Guid locationId, Guid id, CancellationToken cancellationToken);

    Task UpdateDesk(UpdatedDesk updatedDesk, CancellationToken cancellationToken);
    
}