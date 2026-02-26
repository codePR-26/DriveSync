using Microsoft.AspNetCore.Mvc;
using DriveSync.Data;
using DriveSync.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context,
                              IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // -----------------------------
        // PUBLIC USER REGISTER
        // -----------------------------
        [HttpPost("register-user")]
        public IActionResult Register(Account account)
        {
            account.Role = Role.User;

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok("User Registered");
        }


        // -----------------------------
        // CREATE ADMIN (ONLY ParentAdmin)
        // -----------------------------
        [Authorize(Roles = "ParentAdmin")]
        [HttpPost("create-admin")]
        public IActionResult CreateAdmin(Account account)
        {
            account.Role = Role.Admin;

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok("Admin Created");
        }


        // -----------------------------
        // CREATE PARENT ADMIN
        // MAX LIMIT = 2
        // -----------------------------
        [HttpPost("create-parent-admin")]
        public IActionResult CreateParentAdmin(Account account)
        {
            var parentAdminCount =
                _context.Accounts.Count(a =>
                a.Role == Role.ParentAdmin);

            if (parentAdminCount >= 2)
            {
                return BadRequest("Parent Admin limit reached");
            }

            account.Role = Role.ParentAdmin;

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok("Parent Admin Created");
        }


        // -----------------------------
        // LOGIN
        // -----------------------------
        [HttpPost("login")]
        public IActionResult Login(Account loginUser)
        {
            var account = _context.Accounts
                .FirstOrDefault(x =>
                x.Username == loginUser.Username &&
                x.Password == loginUser.Password);

            if (account == null)
                return Unauthorized("Invalid Credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,
                          account.Username),

                new Claim(ClaimTypes.Role,
                          account.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config["Jwt:Key"]));

            var creds =
                new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            var tokenString =
                new JwtSecurityTokenHandler()
                .WriteToken(token);

            Response.Cookies.Append("jwt",
                tokenString,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires =
                    DateTimeOffset.Now.AddHours(2)
                });

            return Ok("Login Successful");
        }


        // -----------------------------
        // LOGOUT
        // -----------------------------
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok("Logged out successfully");
        }
    }
}