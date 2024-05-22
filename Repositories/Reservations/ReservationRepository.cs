using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AuthContext _context;

        public ReservationRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllReservations()
        {
            return await _context.Set<Reservation>().ToListAsync();
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            return await _context.Set<Reservation>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Reservation>> GetAllReservationByUserId(int userId)
        {
            return await _context.Set<Reservation>().Where(r => r.IdUser == userId).ToListAsync();
        }

        public async Task<bool> AddReservation(Reservation reservation)
        {
            _context.Set<Reservation>().Add(reservation);
            await _context.SaveChangesAsync();
            return true; // Assuming success
        }

        public async Task<bool> UpdateReservation(Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true; // Assuming success
        }

        public async Task<bool> DeleteReservation(int id)
        {
            var reservation = await _context.Set<Reservation>().FindAsync(id);
            if (reservation == null)
            {
                return false; // Not found
            }
            _context.Set<Reservation>().Remove(reservation);
            await _context.SaveChangesAsync();
            return true; // Deleted successfully
        }

        public async Task<IEnumerable<Reservation>> GetReservationsAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }


        public async Task AddReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ReservationExistsAsync(DateTime? startDate, DateTime? endDate, int boatId)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                throw new ArgumentException("Start date and end date must have values.");
            }

            return await _context.Reservations
                .AnyAsync(r => r.IdBoat == boatId &&
                               r.StartDate < endDate.Value &&
                               r.EndDate > startDate.Value);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByBoatIdAsync(int boatId)
        {
            // Assuming Reservation has a BoatId property
            return await _context.Reservations
                .Where(r => r.IdBoat == boatId)
                .ToListAsync();
        }
        public async Task<FeedBack> GetFeedbackByUserAndBoatIdAsync(int userId, int boatId)
        {
            return await _context.FeedBacks
                .FirstOrDefaultAsync(f => f.IdUser == userId && f.IdBoat == boatId);
        }
        public async Task<IEnumerable<Boat>> GetAvailableBoatsAsync(DateTime startDate, DateTime endDate, string city)
        {
            var availableBoats = await _context.Boats
                .Where(boat => boat.City == city &&
                               !boat.Reservations.Any(reservation =>
                                   reservation.StartDate < endDate && reservation.EndDate > startDate))
                .ToListAsync();

            return availableBoats;
        }
    }
}
