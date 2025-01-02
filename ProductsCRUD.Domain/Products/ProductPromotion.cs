using ProductsCRUD.Domain.Promotions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsCRUD.Domain.Products
{
    public class ProductPromotion
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }


        [ForeignKey("Promotion")]
        public int PromotionId { get; set; }



        public virtual Product Product { get; set; }
        public virtual Promotion Promotion { get; set; }

    }
}
