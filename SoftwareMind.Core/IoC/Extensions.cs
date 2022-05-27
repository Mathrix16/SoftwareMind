using Microsoft.Extensions.DependencyInjection;
using SoftwareMind.Core.Services;

namespace SoftwareMind.Core.IoC;

public static class Extensions
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<IDesksService, DesksService>();
        services.AddScoped<ILocationsService, LocationsService>();
        services.AddScoped<IReservationsService, ReservationsService>();
    }
}