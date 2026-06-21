using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Shared.Models;

namespace WebApplication1.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ContractsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll(string? status = null)
        {
            var query = _context.Contracts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(x => x.Status == status);

            var result = await query
                .Select(c => new
                {
                    c.ContractId,
                    c.ClientId,
                    c.StartDate,
                    c.EndDate,
                    c.Status,
                    c.ServiceLevel,
                    c.PdfPath
                })
                .ToListAsync();

            return Ok(result);
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _context.Contracts
                .Where(c => c.ContractId == id)
                .Select(c => new
                {
                    c.ContractId,
                    c.ClientId,
                    c.StartDate,
                    c.EndDate,
                    c.Status,
                    c.ServiceLevel,
                    c.PdfPath
                })
                .FirstOrDefaultAsync();

            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        // =========================
        // CREATE CONTRACT (FIXED MODEL ERROR)
        // =========================
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Contract model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            string? filePath = null;

            // =========================
            // FILE UPLOAD
            // =========================
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                var folder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                filePath = "/uploads/" + fileName;
            }

            model.PdfPath = filePath;

            _context.Contracts.Add(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // =========================
        // UPDATE STATUS
        // =========================
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            contract.Status = status;
            await _context.SaveChangesAsync();

            return Ok(contract);
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }
    }
}