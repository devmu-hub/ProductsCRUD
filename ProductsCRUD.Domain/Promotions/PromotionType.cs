using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductsCRUD.Domain.Promotions
{
    public class PromotionType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }




        public virtual ICollection<Promotion> Promotions { get; set; }

    }
}
