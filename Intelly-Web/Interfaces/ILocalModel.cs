using Intelly_Web.Entities;

namespace Intelly_Web.Interfaces
{
    public interface ILocalModel
    {
        Task<ApiResponse<List<LocalEnt>>> GetAllLocals();
        Task<ApiResponse<LocalEnt>> CreateLocal(LocalEnt entity);
        Task<ApiResponse<LocalEnt>> GetSpecificLocal(int LocalId);
        Task<ApiResponse<LocalEnt>> EditSpecificLocal(LocalEnt entity);
        Task<ApiResponse<LocalEnt>> UpdateLocalState(LocalEnt entity);
    }
}
