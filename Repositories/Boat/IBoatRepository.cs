using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IBoatRepository
    {
        Task<Boat> GetByIdAsync(int id);
        Task<List<Boat>?> GetAllBoats();
        Task<Boat> GetByIdBoat(int id);
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> AddBoat(Boat Boat);
        Task<bool> UpdateBoat(Boat Boat);
        Task<bool> DeleteBoat(int id);
    }
}
