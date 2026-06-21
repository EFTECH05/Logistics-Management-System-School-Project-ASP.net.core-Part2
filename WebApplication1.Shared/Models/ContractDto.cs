namespace WebApplication1.Shared.DTOs
{
    // =========================
    // API CONTRACT DATA ONLY
    // (NO FILES, NO EF CORE)
    // =========================
    public class ContractDto
    {
        public int ClientId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ServiceLevel { get; set; } = "Standard";

        public string Status { get; set; } = "Draft";

        // Only path, NOT file upload
        public string? PdfPath { get; set; }
    }
}