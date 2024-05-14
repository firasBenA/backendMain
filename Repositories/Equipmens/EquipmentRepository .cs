using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApi.Models;

namespace TestApi.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AuthContext _context;

        public EquipmentRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipmentsAsync()
        {
            return await _context.Equipments.ToListAsync();
        }

        public async Task<Equipment> GetEquipmentByIdAsync(int id)
        {
            return await _context.Equipments.FindAsync(id);
        }

        public async Task<List<Equipment>> GetAllEquipmentByBoatId(int boatId)
        {
            return await _context.Equipments
                .Where(e => e.IdBoat == boatId) // Filter by boat ID
                .ToListAsync();
        }

        public async Task AddEquipmentAsync(Equipment equipment)
        {
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEquipmentAsync(Equipment equipment)
        {
            _context.Entry(equipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEquipmentAsync(int boatId)
        {
            var equipment = await _context.Equipments.FindAsync(boatId);
            if (equipment == null)
            {
                throw new ArgumentException("Equipment not found.");
            }

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
        }
    }
}
