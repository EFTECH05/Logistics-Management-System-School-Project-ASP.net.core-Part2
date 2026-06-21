using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Shared.Models;
namespace WebApplication1.Shared.Models
{
    public class ServiceRequest
    {
        // =========================
        // PRIMARY KEY
        // =========================
        [Key]
        public int RequestId { get; set; }

        // =========================
        // FOREIGN KEY
        // =========================
        [Required]
        public int ContractId { get; set; }

        // =========================
        // NAVIGATION PROPERTY
        // =========================
        public Contract? Contract { get; set; }

        // =========================
        // BUSINESS DATA
        // =========================
        [Required]
        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostUSD { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostZAR { get; set; }

        // =========================
        // STATUS
        // =========================
        public string Status { get; set; } = "Pending";

        // =========================
        // AUDIT FIELD
        // =========================
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}