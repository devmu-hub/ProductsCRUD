using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsCRUD.Domain.Products
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;




        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }


    }
}
