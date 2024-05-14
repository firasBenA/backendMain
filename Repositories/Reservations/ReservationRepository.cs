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
    }
}
