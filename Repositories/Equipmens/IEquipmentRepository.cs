using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IEquipmentRepository
    {
        Task<IEnumerable<Equipment>> GetAllEquipmentsAsync();
        Task<Equipment> GetEquipmentByIdAsync(int id);
        Task AddEquipmentAsync(Equipment equipment);
        Task UpdateEquipmentAsync(Equipment equipment);
        Task DeleteEquipmentAsync(int id);
        Task<List<Equipment>> GetAllEquipmentByBoatId(int boatId);

    }
}
