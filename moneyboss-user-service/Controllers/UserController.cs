using Microsoft.AspNetCore.Mvc;
using moneyboss_user_service.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace moneyboss_user_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly DbConnection _connection;
        public UserController(DbConnection connection)
        {
            _connection = connection;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            RegisterResponse registerResponse = new RegisterResponse();
            try
            {
                if (_connection.Users.Any(u => u.Username == request.Username))
                {
                    registerResponse.success = true;
                    registerResponse.message = "User already exist!";
                    return BadRequest("User already exist!");
                }

                CreatePasswordHash(request.Password,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);

                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    VerificationToken = CreateToken(),
                };

                _connection.Users.Add(user);
                await _connection.SaveChangesAsync();

                registerResponse.success = true;
                registerResponse.message = "Registration Success.";

                return Ok(registerResponse);
            }
            catch
            {
                registerResponse.success = false;
                registerResponse.message = "Registration Faliur.";
                return BadRequest(registerResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            LoginResponse loginResponse = new LoginResponse();

            var user = await _connection.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if(user == null)
            {
                loginResponse.success = false;
                loginResponse.message = "User not found.";
                return BadRequest(loginResponse);
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                loginResponse.success = false;
                loginResponse.message = "Wrong Password.";
                return BadRequest(loginResponse);
            }

            if (user != null)
            {
                loginResponse.success = true;
                loginResponse.message = "LoginSuccess.";
                return Ok(loginResponse);
            }

            return null;
        }

        private string CreateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
