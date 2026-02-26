using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DriveSync.Data;
using DriveSync.Models;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult AddVehicle(Vehicle vehicle)
        {
            vehicle.CreatedAt = DateTime.Now;
            vehicle.Status ??= "Available";

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return Ok(vehicle);
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
        public IActionResult UpdateVehicle(int id, Vehicle updatedVehicle)
        {
            var vehicle = _context.Vehicles.Find(id);

            if (vehicle == null)
                return NotFound("Vehicle not found");


            vehicle.Brand = updatedVehicle.Brand;
            vehicle.Model = updatedVehicle.Model;

            vehicle.PassengerCapacity =
                updatedVehicle.PassengerCapacity;

            vehicle.EngineCapacity =
                updatedVehicle.EngineCapacity;

            vehicle.DailyRate =
                updatedVehicle.DailyRate;

            vehicle.MonthlyRate =
                updatedVehicle.MonthlyRate;

            vehicle.Status =
                updatedVehicle.Status;


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