using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Contract
    {
        // =========================
        // PRIMARY KEY
        // =========================
        [Key]
        public int ContractId { get; set; }

        // =========================
        // FOREIGN KEY (Client)
        // =========================
        [Required]
        public int ClientId { get; set; }

        // ✅ NAVIGATION PROPERTY
        public Client Client { get; set; }

        // =========================
        // CONTRACT DETAILS
        // =========================
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceLevel { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        // =========================
        // FILE STORAGE (PDF)
        // =========================
        public string? PdfPath { get; set; }

        // =========================
        // RELATIONSHIP (Service Requests)
        // =========================
        public ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}