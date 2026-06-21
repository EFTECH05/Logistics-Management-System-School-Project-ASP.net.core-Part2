using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ContractViewModel
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceLevel { get; set; } = "Standard";

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Draft";

        // Optional file upload (PDF contract)
        public IFormFile? PdfFile { get; set; }
    }
}