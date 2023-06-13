using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiLibrary.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDL { get; set; }

        [Required]
        [StringLength(50)]
        public string? Title { get; set; }

        [Required]
        [StringLength(50)]
        public string? Author { get; set; }

        [Required]
        [StringLength(50)]
        public string? YearPublication { get; set; }

        [Required]
        public int? NumberPage { get; set; }


        [Required]
        public int? Stock { get; set; }
    }
}
