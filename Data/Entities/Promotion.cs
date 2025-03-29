using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_P22.Data.Entities
{
    public record Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal DiscountPercentage { get; set; }

        public List<Product> Products { get; set; } = [];
    }
}
