using MastermindService.AppLogic;
using MastermindService.Data;
using MastermindService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MastermindService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MastermindDBContext db;
        private readonly IConfiguration _configuration;
        private UserLogic userLogic = new UserLogic();

        public UserController(MastermindDBContext db, IConfiguration configuration)
        {
            this.db = db;
            _configuration = configuration;
        }

        // GET: api/<UserController>
        //get All users
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId))
                    {
                        List<AppUserDTO> users = userLogic.getUserList(db);
                        return Ok(users);
                    }
                    else
                    {
                        // userId is null or empty
                        return BadRequest("Invalid user ID");
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                return Unauthorized();
            }
        }

        //get user by ID
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var user = userLogic.getUserByID(id, db);
                        return Ok(user);
                    }
                    else
                    {
                        // userId is null or empty
                        return BadRequest("Invalid user ID");
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                return Unauthorized();
            }
        }

        // function to register a user
        [HttpPost]
        public IActionResult RegisterUser([FromBody] AppUser user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
            user.password = hashedPassword;
            var response = userLogic.registerUser(user, db);
            return Ok(response);
        }

        // function to update user details
        [Authorize]
        [HttpPut]
        public IActionResult UpdateUser([FromBody] AppUser user)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            var response = "";

            // Check if the authorization header is present and has the expected format
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    ClaimsPrincipal principal = ValidateToken(token);
                    string userId = principal.FindFirstValue("Id");
                    if (!string.IsNullOrEmpty(userId) && int.Parse(userId) == user.Id)
                    {
                        response = userLogic.updateUser(user, db);
                        return Ok(response);
                    }
                    else
                    {
                        // userId is null or empty
                        response = "Invalid user ID";
                    }
                }
                catch (Exception ex)
                {
                    // Token validation failed
                    // Handle the exception appropriately
                    response = ex.Message;
                }
            }
            else
            {
                // Authorization header is missing or has an invalid format
                response = "Unauthorised user";
            }
            return Ok(response);
        }

        // login function with token accesss
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult LoginUser([FromBody] AppUser user)
        {
            if (user != null)
            {
                var dbUser = db.AppUser.FirstOrDefault(u => u.username == user.username);
                if (dbUser == null)
                {
                    return BadRequest("Invalid Credentials");
                }

                if (BCrypt.Net.BCrypt.Verify(user.password, dbUser.password))
                {
                    dbUser.authenticationToken = EncryptToken(GetToken(dbUser), _configuration["TOKEN_KEY"]); //Environment.GetEnvironmentVariable("JWT_KEY")
                    return Ok(dbUser);
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest("User Not Found");
            }
        }

        // helper method to generate the token {can add claims and roles}
        private string GetToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
             {
                new Claim("Id", user.Id.ToString()) // Add userID claim
             };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                 claims: claims, // Set the claims for the token
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // token encryption
        private string EncryptToken(string token, string encryptionKey)
        {
            byte[] encryptedBytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.Mode = CipherMode.ECB; // Use ECB mode
                aes.Padding = PaddingMode.PKCS7; // Use PKCS7 padding

                ICryptoTransform encryptor = aes.CreateEncryptor();

                byte[] tokenBytes = Encoding.UTF8.GetBytes(token);

                using (var encryptedStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(tokenBytes, 0, tokenBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    encryptedBytes = encryptedStream.ToArray();
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        //function to validate the token
        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]))
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                // Token validation failed
                // Handle the exception appropriately or throw a custom exception
                throw new Exception("Invalid token", ex);
            }
        }
    }
}