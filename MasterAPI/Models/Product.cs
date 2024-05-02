using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasterAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required] 
        public string Sku { get; set; }
        [Required]
        public decimal Price { get; set; }
        
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public bool IsAvailable { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
