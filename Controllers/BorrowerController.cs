using ApiLibrary.DatabaseClasses;
using ApiLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowerController : ControllerBase
    {
        DbManager db = new DbManager("Data Source=DatabaseFile/Biblitheque.db");

        [HttpGet(Name = "GetAllBorrowers")]
        public IEnumerable<Borrower> Get()
        {
            return db.GetBorrowers();
        }

        [HttpGet("{id}", Name = "GetBorrower")]
        public ActionResult<Borrower> Get(int id)
        {
            var Borrower = db.GetBorroweById(id);
            if (Borrower == null)
            {
                return NotFound();
            }
            return Borrower;
        }

        [HttpPost(Name = "CreateBorrower")]
        public IActionResult Create([FromBody] Borrower borrower)
        {
            if (ModelState.IsValid)
            {
                int newBookId = db.AddBorrower(borrower);
                borrower.BorrowerId = newBookId;
                return CreatedAtRoute("GetBorrower", new { id = newBookId }, borrower);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateBorrower")]
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


        [HttpDelete("{id}", Name = "DeleteBorrower")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteBorrower(id))
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
