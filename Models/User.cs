using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiLibrary.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [StringLength(50)]
        public string? UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string? Password { get; set; }

    }
}
