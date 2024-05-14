using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservations();
        Task<Reservation> GetReservationById(int id);
        Task<List<Reservation>> GetAllReservationByUserId(int userId); // Add this method
        Task<bool> AddReservation(Reservation reservation);
        Task<bool> UpdateReservation(Reservation reservation);
        Task<bool> DeleteReservation(int id);
    }
}
