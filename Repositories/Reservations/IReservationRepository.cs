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
        Task<IEnumerable<Reservation>> GetReservationsByBoatIdAsync(int boatId);
        Task<IEnumerable<Reservation>> GetReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task AddReservationAsync(Reservation reservation);
        Task<bool> ReservationExistsAsync(DateTime? startDate, DateTime? endDate, int boatId);
        Task<IEnumerable<Boat>> GetAvailableBoatsAsync(DateTime startDate, DateTime endDate, string city);

    }
}
