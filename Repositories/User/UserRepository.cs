using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc;

namespace TestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _context;

        public UserRepository(AuthContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByUsernameAndPassword(string email, string password)
        {
            try
            {
                return _context.Users.SingleOrDefault(u => u.Email == email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>?> GetAllUsers()
        {
            try
            {
                List<User> Luser = new();
                Luser = await _context.Users.ToListAsync();
                return Luser;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<User> GetByIdUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return null; // Return null when user is not found
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<bool> AddUser(User user)
        {
            await using IDbContextTransaction transaction = _context.Database.BeginTransaction();


            try
            {
                if (user == null)
                {
                    return false;
                }
                await _context.Users.AddAsync(user);
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

        public async Task<bool> UpdateUser(User user)
        {

            await using IDbContextTransaction transaction = _context.Database.BeginTransaction();

            try
            {
                if (user == null)
                {
                    return false;
                }
                _context.Users.Update(user);
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

        public async Task<string> UploadImageAsync([FromForm] IFormFile file)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                var filePath = Path.Combine("wwwroot/avatar/", fileName);

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
        public async Task<bool> DeleteUser(int id)
        {

            await using IDbContextTransaction transaction = _context.Database.BeginTransaction();

            try
            {
                if (id == 0)
                {
                    return false;
                }
                User? user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return false;
                }
                _context.Users.Remove(user);

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

        public async Task<bool> UserExistsUser(int id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }

        private User NotFound()
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}