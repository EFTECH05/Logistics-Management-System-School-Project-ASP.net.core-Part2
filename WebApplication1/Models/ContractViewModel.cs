using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Shared.Models;
namespace WebApplication1.Models
{
    public class ContractViewModel
    {
        [Required(ErrorMessage = "Client is required")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Service level is required")]
        [StringLength(50)]
        public string ServiceLevel { get; set; } = "Standard";

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50)]
        public string Status { get; set; } = "Draft";

        // ✅ File upload (MVC only)
        public IFormFile? PdfFile { get; set; }

        // =========================
        // OPTIONAL: VALIDATION RULE
        // =========================
        public bool IsValidDateRange()
        {
            return EndDate > StartDate;
        }
    }
}