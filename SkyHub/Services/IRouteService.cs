using SkyHub.Models.Flight_Details;

namespace SkyHub.Services
{
    public interface IRouteService
    {
        Task<IEnumerable<Routes>> GetAllRoutesAsync();
        Task<Routes> GetRouteByIdAsync(int routeId);
        Task AddRouteAsync(Routes newRoute);
        Task UpdateRouteAsync(int routeId, Routes updatedRoute);
        Task DeleteRouteAsync(int routeId);
    }
}
