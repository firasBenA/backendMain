using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestApi.Models;
using TestApi.Repositories;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using TestApi.Helpers;


namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly JwtService _jwtService;


        public UserController(IUserRepository UserRepository, JwtService jwtService)
        {
            _UserRepository = UserRepository;
            _jwtService = jwtService;

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var existingUser = _UserRepository.GetUserByUsernameAndPassword(user.Email, user.Password);

            if (existingUser == null)
            {
                return Unauthorized();
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new { user });
        }

        // GET: api/User/user
        [HttpGet("user")]
        public async Task<IActionResult> User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                // Verify the JWT token
                var token = _jwtService.Verify(jwt);

                // Extract the user ID claim from the validated JWT token
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == "sub");

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var user = await _UserRepository.GetByIdUser(userId);

                    // Check if the user exists
                    if (user != null)
                    {
                        return Ok(user); // Return the user if found
                    }
                }

                return NotFound(); // Return 404 Not Found if user with the specified ID is not found
            }
            catch (Exception)
            {
                return Unauthorized(); // Return 401 Unauthorized for any exception
            }
        }




        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                List<User>? LUser = await _UserRepository.GetAllUsers();
                return Ok(LUser);
            }
            catch
            {
                return Problem();
            }
        }

        // GET: api/User
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetByIdUser(int id)
        {
            var user = await _UserRepository.GetByIdUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {

            try
            {
                if (user == null)
                {
                    return BadRequest();
                }
                bool res = await _UserRepository.AddUser(user);
                return Ok(res);
            }
            catch
            {
                return Problem();
            }

            /* var newUser = await _UserRepository.AddUser(user);
             return CreatedAtAction(nameof(AddUser), new { id = newUser.Id }, newUser);*/
        }

        // PUT: api/User
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {

            try
            {
                if (user == null)
                {
                    return BadRequest();
                }
                bool res = await _UserRepository.UpdateUser(user);
                return Ok(res);
            }
            catch
            {
                return Problem();

            }

        }

        [HttpPut("/updateAvatarImage/{userId}")]
        public async Task<IActionResult> UpdateAvatarImage(int userId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is null or empty.");

                var avatar = await _UserRepository.UploadImageAsync(file);
                avatar = avatar.Replace("wwwroot", "");
                avatar = "/" + avatar;
                var user = await _UserRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    user.Avatar = avatar;
                    await _UserRepository.UpdateUser(user);
                }
                else
                {
                    return NotFound("user not found.");
                }

                return Ok(new { user, avatar });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // DELETE: api/User
        /* [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteUser(int id)
         {
             try
             {
                 if (id <= 0)
                 {
                     return BadRequest();
                 }
                 bool result = await _UserRepository.DeleteUser(id);
                 if (!result)
                 {
                     return NotFound();
                 }
                 return NoContent();
             }
             catch
             {
                 return Problem();
             }
         }*/

        // DELETE: api/User
        [HttpDelete("{id}")]
        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return false;
                }

                var user = await _UserRepository.GetByIdUser(id);

                if (user == null)
                {
                    return false; // User not found
                }

                user.Active = 0; 

                await _UserRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


    }
}
