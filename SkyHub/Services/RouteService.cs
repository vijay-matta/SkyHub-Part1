using SkyHub.Data;
using SkyHub.Models.Flight_Details;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyHub.Services
{
    
    public class RouteService : IRouteService
    {
        private readonly SkyHubDbContext _context;

        public RouteService(SkyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Routes>> GetAllRoutesAsync()
        {
            return await _context.Routes.Include(r => r.FlightOwner).ToListAsync();
        }

        public async Task<Routes> GetRouteByIdAsync(int routeId)
        {
            return await _context.Routes.Include(r => r.FlightOwner)
                                        .FirstOrDefaultAsync(r => r.RouteId == routeId);
        }

        public async Task AddRouteAsync(Routes newRoute)
        {
            _context.Routes.Add(newRoute);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRouteAsync(int routeId, Routes updatedRoute)
        {
            var route = await GetRouteByIdAsync(routeId);
            if (route != null)
            {
                route.Origin = updatedRoute.Origin;
                route.Destination = updatedRoute.Destination;
                route.Duration = updatedRoute.Duration;
                route.Distance = updatedRoute.Distance;

                _context.Routes.Update(route);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRouteAsync(int routeId)
        {
            var route = await GetRouteByIdAsync(routeId);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
        }
    }
}
