using Microsoft.AspNetCore.Mvc;
using DriveSync.Data;
using DriveSync.Models;
using Microsoft.AspNetCore.Authorization;

namespace DriveSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Must login
    public class VehicleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------
        // CREATE VEHICLE
        // ADMIN ONLY
        // ------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return Ok(vehicle);
        }


        // ------------------------
        // GET ALL VEHICLES
        // ALL ROLES
        // ------------------------
        [Authorize(Roles = "Admin,User,ParentAdmin")]
        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            var vehicles = _context.Vehicles.ToList();

            return Ok(vehicles);
        }


        // ------------------------
        // GET VEHICLE BY ID
        // ALL ROLES
        // ------------------------
        [Authorize(Roles = "Admin,User,ParentAdmin")]
        [HttpGet("{id}")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _context.Vehicles.Find(id);

            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }


        // ------------------------
        // UPDATE VEHICLE
        // ADMIN ONLY
        // ------------------------
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateVehicle(int id,
                                           Vehicle updatedVehicle)
        {
            var vehicle = _context.Vehicles.Find(id);

            if (vehicle == null)
                return NotFound();

            vehicle.Brand = updatedVehicle.Brand;
            vehicle.Model = updatedVehicle.Model;
            vehicle.Year = updatedVehicle.Year;
            vehicle.PricePerDay =
                updatedVehicle.PricePerDay;
            vehicle.IsAvailable =
                updatedVehicle.IsAvailable;

            _context.SaveChanges();

            return Ok(vehicle);
        }


        // ------------------------
        // DELETE VEHICLE
        // ADMIN ONLY
        // ------------------------
        [Authorize(Roles = "Admin")]
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