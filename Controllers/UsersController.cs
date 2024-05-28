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
using Microsoft.AspNetCore.Identity;
using EmailApi.Repositories;


namespace TestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _UserRepository;
        private readonly JwtService _jwtService;
        private readonly IEmailRepository _emailRepository;



        public UserController(IUserRepository UserRepository, JwtService jwtService, IEmailRepository emailRepository)
        {
            _UserRepository = UserRepository;
            _jwtService = jwtService;
            _emailRepository = emailRepository;

        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var hashedPassword = new PasswordHasher<User>().HashPassword(user, user.Password);
                user.Password = hashedPassword;

                user.Active = 0; // Set to inactive initially
                user.DateInscription = DateTime.UtcNow;

                var verificationCode = GenerateVerificationCode();
                user.VerificationCode = verificationCode;

                bool res = await _UserRepository.AddUser(user);

                await _emailRepository.SendVerificationEmailAsync(user.Email, verificationCode);

                return Ok(new { res, HashedPassword = hashedPassword });
            }
            catch
            {
                return Problem();
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationModel model)
        {
            var user = await _UserRepository.FindByEmailAsync(model.Email);

            if (user == null || user.VerificationCode != model.VerificationCode)
            {
                return BadRequest("Invalid verification code.");
            }

            user.Active = 1; // Activate the user
            user.VerificationCode = null; // Clear the verification code

            await _UserRepository.UpdateUser(user);

            return Ok("Email verified successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userLogin)
        {
            var existingUser = await _UserRepository.FindByEmailAsync(userLogin.Email);

            if (existingUser == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Compare the provided plain text password with the hashed password
            var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(existingUser, existingUser.Password, userLogin.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Generate JWT token
            var jwt = _jwtService.Generate(existingUser.Id);

            // Set JWT token in cookie
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                // Set other cookie options if needed (e.g., expiration)
            });

            // Return only necessary user data (excluding sensitive info like password)
            return Ok(new
            {
                existingUser,
                userId = existingUser.Id,
                email = existingUser.Email,
                jwt
            });
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
        /*[HttpPost]
        public async Task<ActionResult> AddUsersssss(User user)
        {

            try
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var hashedPassword = new PasswordHasher<User>().HashPassword(user, user.Password);
                user.Password = hashedPassword;

                user.Active = 1;

                user.DateInscription = DateTime.UtcNow;
                bool res = await _UserRepository.AddUser(user);
                return Ok(new { res, HashedPassword = hashedPassword });
            }
            catch
            {
                return Problem();
            }

            /* var newUser = await _UserRepository.AddUser(user);
             return CreatedAtAction(nameof(AddUser), new { id = newUser.Id }, newUser);*/
        //} //

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

       

        [HttpPut("{userId}/block")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            try
            {
                var user = await _UserRepository.GetByIdUser(userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.Active == 0)
                {
                    return BadRequest("User is already blocked.");
                }

                user.Active = 0; // Assuming 0 means blocked, adjust as per your logic
                await _UserRepository.UpdateUser(user);

                return Ok(new { message = "User blocked successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error blocking user: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{userId}/unblock")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            try
            {
                var user = await _UserRepository.GetByIdUser(userId);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.Active == 1)
                {
                    // User is already active
                    return BadRequest("User is already active.");
                }

                user.Active = 1; // Assuming 1 means active, adjust as per your logic
                await _UserRepository.UpdateUser(user);

                return Ok(new { message = "User unblocked successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unblocking user: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
