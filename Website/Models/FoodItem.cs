using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class FoodItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; } = new byte[0];
    }
}
