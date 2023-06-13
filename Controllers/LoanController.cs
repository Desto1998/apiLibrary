using ApiLibrary.DatabaseClasses;
using ApiLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Biblitheque.db");

        [HttpGet(Name = "GetAllLoans")]
        public IEnumerable<Loan> Get()
        {
            return db.GetLoans();
        }



        [HttpGet("{id}", Name = "GetLoan")]
        public bool Get(int id)
        {
            var loand = db.returBook(id);
            if (loand == true)
            {
                return true;
            }
            return false;
        }

        [HttpPost(Name = "CreateLoan")]
        public IActionResult Create([FromBody] Loan loan)
        {
            if (ModelState.IsValid)
            {
                int newLoanId = db.AddLoan(loan);
                loan.BorrowerId = newLoanId;
                return CreatedAtRoute("GetAllLoans", new { id = newLoanId }, loan);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateLoan")]
        public IActionResult Put(int id, [FromBody] Borrower borrower)
        {
            if (borrower == null || borrower.BorrowerId != id)
            {
                return BadRequest();
            }

            var existingBorrower = db.GetBorroweById(id);
            if (existingBorrower == null)
            {
                return NotFound();
            }

            existingBorrower.FirstName = borrower.FirstName;
            existingBorrower.LastName = borrower.LastName;
            existingBorrower.PhoneNumber = borrower.PhoneNumber;
            existingBorrower.Email = borrower.Email;

            existingBorrower.Address.Number = borrower.Address.Number;
            existingBorrower.Address.Street = borrower.Address.Street;
            existingBorrower.Address.ZipCode = borrower.Address.ZipCode;
            existingBorrower.Address.Country = borrower.Address.Country;
            existingBorrower.Address.Land = borrower.Address.Land;

            db.UpdateBorrower(existingBorrower);

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteLoan")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteLoan(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpGet("{address}", Name = "GetByNamesOrAddress")]
        //public IEnumerable<Borrower> GetByNamesOrAddress(string address)
        //{
        //    return db.GetByNamesOrAddress(address);
        //}

        //[HttpGet("{address}", Name = "GetBorrowersByKeyword")]
        //public IEnumerable<Borrower> GetBorrowersByKeyword(string address)
        //{
        //    return db.GetBorrowersByKeyword(address);
        //}

        //[HttpGet("{address}", Name = "GetBorrowerOfAddress")]
        //public IEnumerable<Borrower> GetBorrowerOfAddress(string address)
        //{
        //    return db.GetBorrowerOfAddress(address);
        //}

        //[HttpGet("{bookId}", Name = "GetBookBorrower")]
        //public IEnumerable<Borrower> GetBookBorrower(int bookId)
        //{
        //    return db.GetBookBorrower(bookId);

        //}
    }
}
