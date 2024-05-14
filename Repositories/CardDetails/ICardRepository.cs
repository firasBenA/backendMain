using System.Threading.Tasks;
using TestApi.Models;

namespace TestApi.Repositories
{
    public interface ICardRepository
    {
        Task<bool> ValidateCard(CardDetails cardDetails);
    }
}
