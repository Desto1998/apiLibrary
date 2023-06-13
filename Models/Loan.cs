using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiLibrary.Models
{
    public class Loan
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanId { get; set; }

        [Required]
        public string BorrowedDate { get; set; }

        [Required]
        public string ReturnDate { get; set; }

        [Required]
        public int IsRetuned { get; set; }

        [Required]
        public int BorrowerId { get; set; }
        
        [Required]
        public int IDL { get; set; }

        [ForeignKey("BorrowerId")]
        public virtual Borrower? Borrower { get; set; }
        
        [ForeignKey("IDL")]
        public virtual Book? Book { get; set; }


    }
}
