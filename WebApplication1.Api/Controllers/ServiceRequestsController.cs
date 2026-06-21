using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Shared.Models;

namespace WebApplication1.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServiceRequestsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ServiceRequests
                .AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.ServiceRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RequestId == id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Invalid data");

            var contract = await _context.Contracts.FindAsync(request.ContractId);

            if (contract == null)
                return NotFound("Contract not found");

            if (contract.Status == "Expired")
                return BadRequest("Contract expired");

            if (contract.Status == "On Hold")
                return BadRequest("Contract on hold");

            request.Contract = null;
            request.Status = "Pending";
            request.CreatedDate = DateTime.Now;

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ServiceRequests.FindAsync(id);

            if (item == null)
                return NotFound();

            _context.ServiceRequests.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}