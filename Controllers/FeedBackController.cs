using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackRepository _feedBackRepository;

        public FeedBackController(IFeedBackRepository feedBackRepository)
        {
            _feedBackRepository = feedBackRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetAllFeedBack()
        {
            var feedBacks = await _feedBackRepository.GetAllFeedBackAsync();
            return Ok(feedBacks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedBack>> GetFeedBackById(int id)
        {
            var feedBack = await _feedBackRepository.GetFeedBackByIdAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }
            return Ok(feedBack);
        }

        [HttpPost]
        public async Task<ActionResult<FeedBack>> AddFeedBack(FeedBack feedBack)
        {
            await _feedBackRepository.AddFeedBackAsync(feedBack);
            return CreatedAtAction(nameof(GetFeedBackById), new { id = feedBack.Id }, feedBack);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedBack(int id, FeedBack feedBack)
        {
            if (id != feedBack.Id)
            {
                return BadRequest();
            }

            await _feedBackRepository.UpdateFeedBackAsync(feedBack);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedBack(int id)
        {
            await _feedBackRepository.DeleteFeedBackAsync(id);
            return NoContent();
        }

        [HttpGet("average")]
        public async Task<ActionResult<double>> GetAverageRating()
        {
            var averageRating = await _feedBackRepository.CalculateAverageRatingAsync();
            return Ok(averageRating);
        }

        [HttpGet("ByBoatId/{boatId}")]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedbacksByBoatId(int boatId)
        {
            var feedbacks = await _feedBackRepository.GetFeedbacksByBoatId(boatId);

            if (feedbacks == null || feedbacks.Count == 0)
            {
                return NotFound();
            }

            return Ok(feedbacks);
        }

        [HttpGet("AverageRatingByBoatId/{boatId}")]
        public async Task<ActionResult<double?>> GetAverageRatingByBoatId(int boatId)
        {
            var averageRating = await _feedBackRepository.GetAverageRatingByBoatIdAsync(boatId);

            if (averageRating == null)
            {
                return NotFound(); // Or return a custom response for no ratings found
            }

            return Ok(averageRating); // Returns the average rating
        }

        [HttpGet("ByUserAndBoatId/{userId}/{boatId}")]
        public async Task<ActionResult<FeedBack>> GetFeedbackByUserAndBoatId(int userId, int boatId)
        {
            var feedback = await _feedBackRepository.GetFeedbackByUserAndBoatIdAsync(userId, boatId);

            if (feedback == null)
            {
                return Ok(0); // Or return a custom response for no feedback found
            }

            return Ok(feedback); // Returns the existing feedback
        }
    }
}
