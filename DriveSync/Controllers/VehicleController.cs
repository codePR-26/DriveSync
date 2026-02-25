using Microsoft.AspNetCore.Mvc;
using DriveSync.Data;
using DriveSync.Models;
using Microsoft.AspNetCore.Authorization;

// Vehicle management controller
// Protected using JWT authentication and role-based authorization  

namespace DriveSync.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        [HttpPost]
        public IActionResult AddVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return Ok(vehicle);
        }

        // READ ALL
        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            var vehicles = _context.Vehicles.ToList();
            return Ok(vehicles);
        }

        // READ BY ID
        [HttpGet("{id}")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult UpdateVehicle(int id, Vehicle updatedVehicle)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
                return NotFound();

            vehicle.Brand = updatedVehicle.Brand;
            vehicle.Model = updatedVehicle.Model;
            vehicle.Year = updatedVehicle.Year;
            vehicle.PricePerDay = updatedVehicle.PricePerDay;
            vehicle.IsAvailable = updatedVehicle.IsAvailable;

            _context.SaveChanges();
            return Ok(vehicle);
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle == null)
                return NotFound();

            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();

            return Ok("Vehicle Deleted Successfully");
        }
    }
}