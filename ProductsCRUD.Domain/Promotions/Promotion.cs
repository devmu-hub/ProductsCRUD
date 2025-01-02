using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProductsCRUD.Domain.Products;

namespace ProductsCRUD.Domain.Promotions
{
    public class Promotion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [ForeignKey("PromotionType")]
        public int PromotionTypeId { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountPercentage { get; set; }





        public virtual PromotionType PromotionType { get; set; }

        public virtual ICollection<ProductPromotion> ProductPromotions { get; set; }

    }
}
