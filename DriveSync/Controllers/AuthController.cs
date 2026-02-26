using Microsoft.AspNetCore.Mvc;
using DriveSync.Data;
using DriveSync.Models;
using DriveSync.DTOs;
using Microsoft.IdentityModel.Tokens; // ✅ ADDED JWT
using System.IdentityModel.Tokens.Jwt; // ✅ ADDED JWT
using System.Security.Claims; // ✅ ADDED CLAIMS
using System.Text; // ✅ ADDED SECRET KEY

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        // ============================
        // REGISTER CUSTOMER
        // ============================
        [HttpPost("register-customer")]
        public IActionResult RegisterCustomer(Account account)
        {
            if (_context.Accounts.Any(a =>
                a.Email == account.Email))
            {
                return BadRequest("Email already exists");
            }

            account.Role = "Customer";

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok("Customer Registered");
        }



        // ============================
        // REGISTER ADMIN
        // ============================
        [HttpPost("register-admin")]
        public IActionResult RegisterAdmin(Account account)
        {
            if (_context.Accounts.Any(a =>
                a.Email == account.Email))
            {
                return BadRequest("Email already exists");
            }

            account.Role = "Admin";

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok("Admin Created");
        }



        // ============================
        // REGISTER OWNER (MAX 2)
        // ============================
        [HttpPost("register-owner")]
        public IActionResult RegisterOwner(Account account)
        {
            var ownerCount =
            _context.Accounts
            .Count(a => a.Role == "Owner");

            if (ownerCount >= 2)
            {
                return BadRequest(
                "Maximum 2 Owners allowed");
            }

            if (_context.Accounts.Any(a =>
                a.Email == account.Email))
            {
                return BadRequest(
                "Email already exists");
            }

            account.Role = "Owner";

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return Ok("Owner Created");
        }



 // ============================
 // LOGIN (COMMON)
 // ============================

        //    POST /api/Auth/login
        //          ↓

        //   email + password check

        //         ↓

        //   JWT token generated

        //       ↓

        //   Stored in cookie "jwt"

        //       ↓

        //   Postman automatically sends cookie

        //           ↓

        //   VehicleController Authorize works





        [HttpPost("login")]
        public IActionResult Login(LoginDTOs loginUser)
        {
            var account =
            _context.Accounts.FirstOrDefault(a =>
            a.Email == loginUser.Email &&
            a.Password == loginUser.Password);

            if (account == null)
            {
                return Unauthorized(
                "Invalid Email or Password");
            }


            // ✅ ADDED — JWT CLAIMS
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, account.Email),

        // ✅ VERY IMPORTANT FOR ROLE AUTHORIZE
        new Claim(ClaimTypes.Role, account.Role),

        new Claim("UserId", account.Id.ToString())
    };


            // ✅ ADDED — SECRET KEY FROM appsettings.json
            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                    HttpContext.RequestServices
                    .GetRequiredService<IConfiguration>()
                    ["Jwt:Key"]!));

            var creds =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);


            // ✅ CREATE TOKEN
            var token = new JwtSecurityToken(

                issuer:
                HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()
                ["Jwt:Issuer"],

                audience:
                HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()
                ["Jwt:Audience"],

                claims: claims,

                expires:
                DateTime.Now.AddHours(3),

                signingCredentials: creds
            );


            var jwtToken =
                new JwtSecurityTokenHandler()
                .WriteToken(token);


            // ✅ ADDED — STORE TOKEN INSIDE COOKIE
            Response.Cookies.Append(
                "jwt",
                jwtToken,
                new CookieOptions
                {
                    HttpOnly = true, // ✅ secure
                    Secure = true,
                    SameSite = SameSiteMode.None,

                    Expires =
                    DateTimeOffset.Now.AddHours(3)
                });


            // OLD RESPONSE KEPT SAME STYLE
            return Ok(new
            {
                Message = "Login Successful",

                account.Name,
                account.Email,
                account.Role
            });
        }

    }
}