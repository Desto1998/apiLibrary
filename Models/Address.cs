using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiLibrary.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }

        [Required]
        public int? Number { get; set; }

        [Required]
        [StringLength(50)]
        public string? Street { get; set; }

        [Required]
        [StringLength(50)]
        public string? ZipCode { get; set; }

        [Required]
        [StringLength(50)]
        public string? Country { get; set; }

        [Required]
        [StringLength(50)]
        public string? Land { get; set; }
    }
}
