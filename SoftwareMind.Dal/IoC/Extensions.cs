using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoftwareMind.Dal.Repositories;

namespace SoftwareMind.Dal.IoC;

public static class Extensions
{
    public static void AddDal(this IServiceCollection services)
    {
        services.AddScoped<IDesksRepository, DesksRepository>();
        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<IReservationsRepository, ReservationsRepository>();
        services.AddDbContext<ApplicationDbContext>(options => { options.UseInMemoryDatabase("InMemoryDb"); });
    }
}