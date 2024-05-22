using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface IFeedBackRepository
    {
        Task<IEnumerable<FeedBack>> GetAllFeedBackAsync();
        Task<FeedBack> GetFeedBackByIdAsync(int id);
        Task AddFeedBackAsync(FeedBack feedBack);
        Task UpdateFeedBackAsync(FeedBack feedBack);
        Task DeleteFeedBackAsync(int id);
        Task<double> CalculateAverageRatingAsync();
        Task<List<FeedBack>> GetFeedbacksByBoatId(int boatId);
        Task<double?> GetAverageRatingByBoatIdAsync(int boatId);
        Task<FeedBack> GetFeedbackByUserAndBoatIdAsync(int userId, int boatId);

    }
}
