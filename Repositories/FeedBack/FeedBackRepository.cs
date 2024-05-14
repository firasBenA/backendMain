using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly AuthContext _context;

        public FeedBackRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FeedBack>> GetAllFeedBackAsync()
        {
            return await _context.FeedBacks.ToListAsync();
        }

        public async Task<FeedBack> GetFeedBackByIdAsync(int id)
        {
            return await _context.FeedBacks.FindAsync(id);
        }

        public async Task AddFeedBackAsync(FeedBack feedBack)
        {
            _context.FeedBacks.Add(feedBack);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeedBackAsync(FeedBack feedBack)
        {
            _context.Entry(feedBack).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeedBackAsync(int id)
        {
            var feedBack = await _context.FeedBacks.FindAsync(id);
            _context.FeedBacks.Remove(feedBack);
            await _context.SaveChangesAsync();
        }

        public async Task<double> CalculateAverageRatingAsync()
        {
            var averageRating = await _context.FeedBacks
                .Where(f => f.Rating != null)
                .AverageAsync(f => f.Rating.Value);
            return averageRating;
        }
         public async Task<List<FeedBack>> GetFeedbacksByBoatId(int boatId)
        {
            return await _context.FeedBacks
                .Where(f => f.IdBoat == boatId)
                .ToListAsync();
        }
    }
}
