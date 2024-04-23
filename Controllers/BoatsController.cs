using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoatController : ControllerBase
    {
        private readonly IBoatRepository _boatRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BoatController(IBoatRepository boatRepository, IWebHostEnvironment webHostEnvironment)
        {
            _boatRepository = boatRepository;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpGet]
        public async Task<ActionResult<List<Boat>>> GetAllBoats()
        {
            var boats = await _boatRepository.GetAllBoats();
            if (boats == null)
            {
                return NotFound(); // Returns 404 if no boats are found
            }
            return boats;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Boat>> GetBoatById(int id)
        {
            var boat = await _boatRepository.GetByIdBoat(id);
            if (boat == null)
            {
                return NotFound(); // Returns 404 if boat with the given id is not found
            }
            return boat;
        }

        [HttpPut("/updateBoatImage/{boatId}")]
        public async Task<IActionResult> UpdateBoatImage(int boatId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is null or empty.");

                var imageUrl = await _boatRepository.UploadImageAsync(file);
                imageUrl = imageUrl.Replace("wwwroot", ""); 
                imageUrl = "/" + imageUrl;
                var boat = await _boatRepository.GetByIdBoat(boatId);
                if (boat != null)
                {
                    boat.ImageUrl = imageUrl;
                    await _boatRepository.UpdateBoat(boat);
                }
                else
                {
                    return NotFound("Boat not found.");
                }

                return Ok(new { boat, imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("image/{imageName}")]
        public IActionResult GetImage(string imageName)
        {

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageName);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageData = System.IO.File.ReadAllBytes(imagePath);
            return File(imageData, "image/jpeg");

        }

        [HttpPost]
        public async Task<ActionResult<Boat>> AddBoat(Boat boat)
        {

            var result = await _boatRepository.AddBoat(boat);
            if (!result)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetBoatById), new { id = boat.Id }, boat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoat(int id, Boat boat)
        {
            if (id != boat.Id)
            {
                return BadRequest();
            }

            var result = await _boatRepository.UpdateBoat(boat);
            if (!result)
            {
                return NotFound(); // Returns 404 if boat with the given id is not found
            }
            return NoContent(); // Returns 204 if boat update is successful
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoat(int id)
        {
            var result = await _boatRepository.DeleteBoat(id);
            if (!result)
            {
                return NotFound(); // Returns 404 if boat with the given id is not found
            }
            return NoContent(); // Returns 204 if boat deletion is successful
        }
    }
}
