using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLibrary.Models
{
    public class Borrower
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BorrowerId { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set;}

        // gerer clé étrangère
        [Required]
        public int AddressId { get; set; }


        [Required]
        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string? Email { get; set; }

        [ForeignKey("AddressId")]
        public virtual Address? Address { get; set; }
}
}
