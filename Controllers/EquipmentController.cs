using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Repositories;

namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        // GET: api/equipment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipments()
        {
            var equipments = await _equipmentRepository.GetAllEquipmentsAsync();
            return Ok(equipments);
        }

        // GET: api/equipment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var equipment = await _equipmentRepository.GetEquipmentByIdAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        // POST: api/equipment
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            await _equipmentRepository.AddEquipmentAsync(equipment);

            return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, equipment);
        }

        // PUT: api/equipment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return BadRequest();
            }

            await _equipmentRepository.UpdateEquipmentAsync(equipment);

            return NoContent();
        }

        // DELETE: api/equipment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            await _equipmentRepository.DeleteEquipmentAsync(id);

            return NoContent();
        }

        [HttpGet("equipment/boat/{boatId}")]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByBoatId(int boatId)
        {
            var equipment = await _equipmentRepository.GetAllEquipmentByBoatId(boatId);
            if (equipment == null || !equipment.Any())
            {
                return NotFound(); // Returns 404 if no equipment is found for the given boat ID
            }
            return equipment;
        }
    }
}
