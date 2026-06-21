using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Shared.Models;
namespace WebApplication1.Models
{
    public class Client
    {
        // =========================
        // PRIMARY KEY
        // =========================
        [Key]
        public int ClientId { get; set; }

        // =========================
        // BUSINESS FIELDS
        // =========================
        [Required(ErrorMessage = "Client name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact details are required")]
        [StringLength(200)]
        public string ContactDetails { get; set; } = string.Empty;

        [Required(ErrorMessage = "Region is required")]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        // =========================
        // RELATIONSHIP (FIXES YOUR ERROR)
        // =========================
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    }
}