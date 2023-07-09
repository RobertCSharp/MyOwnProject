using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string AddedBy { get; set; } = string.Empty;

        [ForeignKey("ItemFood")]
        public int ItemID { get; set; }
        public FoodItem ItemFood { get; set; } = default!;
    }
}
