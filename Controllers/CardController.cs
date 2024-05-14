using Microsoft.AspNetCore.Mvc;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;

        public CardController(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        [HttpPost("checkcard")]
        public async Task<IActionResult> CheckCard([FromBody] CardDetails cardDetails)
        {
            var isValid = await _cardRepository.ValidateCard(cardDetails);

            if (isValid)
            {
                return Ok(new { message = "Card is valid" });
            }
            else
            {
                return BadRequest(new { message = "Card is not valid" });
            }
        }
    }

    
}
