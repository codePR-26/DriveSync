using Microsoft.AspNetCore.Mvc;
using DriveSync.Data;
using DriveSync.Models;
using DriveSync.DTOs;

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