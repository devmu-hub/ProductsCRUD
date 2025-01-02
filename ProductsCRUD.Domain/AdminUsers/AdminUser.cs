using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductsCRUD.Domain.AdminUsers
{
    public class AdminUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string UserName { get; set; }


        [Required]
        [Column(TypeName = "text")]
        public string Password { get; set; }

        public bool IsActive { get; set; }


    }
}
