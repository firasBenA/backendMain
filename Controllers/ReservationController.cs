using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            var reservations = await _reservationRepository.GetReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationByUserId(int userId)
        {
            var reservations = await _reservationRepository.GetAllReservationByUserId(userId);
            return Ok(reservations);
        }

        /*[HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
        {
            await _reservationRepository.AddReservation(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }*/

        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            if (reservation.StartDate == null || reservation.EndDate == null || reservation.IdBoat == null)
            {
                return BadRequest("Start date, end date, and boat ID are required.");
            }

            if (await _reservationRepository.ReservationExistsAsync(reservation.StartDate.Value, reservation.EndDate.Value, reservation.IdBoat.Value))
            {
                return BadRequest("The selected dates are already reserved for this boat.");
            }

            await _reservationRepository.AddReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, reservation);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return BadRequest();
            }

            var existingReservation = await _reservationRepository.GetReservationById(id);
            if (existingReservation == null)
            {
                return NotFound();
            }

            await _reservationRepository.UpdateReservation(reservation);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _reservationRepository.GetReservationById(id);
            if (reservation == null)
            {
                return NotFound();
            }

            await _reservationRepository.DeleteReservation(id);

            return NoContent();
        }

        [HttpGet("boat/{boatId}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByBoatId(int boatId)
        {
            var reservations = await _reservationRepository.GetReservationsByBoatIdAsync(boatId);
            return Ok(reservations);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Boat>>> GetAvailableBoats(DateTime startDate, DateTime endDate, string city)
        {
            var availableBoats = await _reservationRepository.GetAvailableBoatsAsync(startDate, endDate, city);
            return Ok(availableBoats);
        }
    }
}
