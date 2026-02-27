using DriveSync.Data;
using DriveSync.DTOS;
using DriveSync.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Login required
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----------------------------
        // ADD VEHICLE
        // ADMIN ONLY
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddVehicle(CreateVehicleRequestDto request)
        {

            var newVehivle = new Vehicle()
            {
                Model = request.Model,
                Brand = request.Brand,
                PassengerCapacity = request.PassengerCapacity,
                EngineCapacity = request.EngineCapacity,
                DailyRate = request.DailyRate,
                MonthlyRate = request.MonthlyRate,
            };

            newVehivle.CreatedAt = DateTime.Now;
            newVehivle.Status ??= "Available";

            _context.Vehicles.Add(newVehivle);
            _context.SaveChanges();

            return Ok(newVehivle);
        }


        // ----------------------------
        // GET ALL VEHICLES
        // ALL ROLES
        // ----------------------------
        [Authorize(Roles = "Admin,User,ParentAdmin")]
        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            var vehicles = _context.Vehicles.ToList();

            return Ok(vehicles);
        }


        // ----------------------------
        // GET VEHICLE BY ID
        // ALL ROLES
        // ----------------------------
        [Authorize(Roles = "Admin,User,ParentAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);

            if (vehicle == null)
                return NotFound("Vehicle not found");

            return Ok(vehicle);
        }


        // ----------------------------
        // UPDATE VEHICLE
        // ADMIN ONLY
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateVehicle(int id, UpdateVehicleRequestDto  request)
        {
            var vehicle = _context.Vehicles.Find(id);

            if (vehicle == null)
                return NotFound("Vehicle not found");


            vehicle.Brand = request.Brand;
            vehicle.Model = request.Model;

            vehicle.PassengerCapacity =
                request.PassengerCapacity;

            vehicle.EngineCapacity =
                request.EngineCapacity;

            vehicle.DailyRate =
                request.DailyRate;

            vehicle.MonthlyRate =
                request.MonthlyRate;

            vehicle.Status =
                request.Status;


            _context.SaveChanges();

            return Ok(vehicle);
        }


        // ----------------------------
        // DELETE VEHICLE
        // ADMIN ONLY
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);

            if (vehicle == null)
                return NotFound("Vehicle not found");

            _context.Vehicles.Remove(vehicle);

            _context.SaveChanges();

            return Ok("Vehicle Deleted Successfully");
        }
    }
}