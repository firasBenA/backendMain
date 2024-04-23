using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;
using TestApi.Models; // Adjust the namespace as per your project structure

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;

namespace TestApi.Repositories
{
    public class BoatRepository : IBoatRepository
    {
        private readonly AuthContext _context;

        public BoatRepository(AuthContext context)
        {
            _context = context;
        }
        
        public async Task<List<Boat>?> GetAllBoats()
        {
            try
            {
                List<Boat> LBoat = new();
                LBoat = await _context.Boats.ToListAsync();
                return LBoat;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Boat> GetByIdBoat(int id)
        {
            try
            {
                var Boat = await _context.Boats.FindAsync(id);

                if (Boat == null)
                {
                    return null; // Return null when user is not found
                }

                return Boat;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Boat> GetByIdAsync(int id)
        {
            return await _context.Boats.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);


                
                var filePath = Path.Combine("wwwroot/images/", fileName); 

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to upload image.", ex);
            }
        }
        public async Task<bool> AddBoat(Boat Boat)
        {
            await using IDbContextTransaction transaction = _context.Database.BeginTransaction();


            try
            {
                if (Boat == null)
                {
                    return false;
                }
                await _context.Boats.AddAsync(Boat);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;



            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                return false;
            }
            /*_context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;*/
        }

        public async Task<bool> UpdateBoat(Boat Boat)
        {

            await using IDbContextTransaction transaction = _context.Database.BeginTransaction();

            try
            {
                if (Boat == null)
                {
                    return false;
                }
                _context.Boats.Update(Boat);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                return false;
            }


        }

        public async Task<bool> DeleteBoat(int id)
        {

            await using IDbContextTransaction transaction = _context.Database.BeginTransaction();

            try
            {
                if (id == 0)
                {
                    return false;
                }
                Boat? Boat = await _context.Boats.FindAsync(id);
                if (Boat == null)
                {
                    return false;
                }
                _context.Boats.Remove(Boat);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(e);
                return false;
            }
            /* var user = await _context.Users.FindAsync(id);
             if (user == null)
                 return false;

             _context.Users.Remove(user);
             await _context.SaveChangesAsync();
             return true;*/
        }


        private User NotFound()
        {
            throw new NotImplementedException();
        }
    }
}
