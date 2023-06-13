using ApiLibrary.DatabaseClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiLibrary.Models;


namespace ApiLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        DbManager db = new DbManager("Data Source=DatabaseFile/Biblitheque.db");

        [HttpGet(Name = "GetAllBooks")]
        public IEnumerable<Book> Get()
        {
            return db.GetBooks();
        }

        [HttpGet("{id}", Name = "GetBook")]
        public ActionResult<Book> Get(int id)
        {
            var Book = db.GetBookById(id);
            if (Book == null)
            {
                return NotFound();
            }
            return Book;
        }

        [HttpPost(Name = "CreateBook")]
        public IActionResult Create([FromBody] Book Book)
        {
            if (ModelState.IsValid)
            {
                int newBookId = db.AddBook(Book);
                Book.IDL = newBookId;
                return CreatedAtRoute("GetBook", new { id = newBookId }, Book);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}", Name = "UpdateBook")]
        public IActionResult Put(int id, [FromBody] Book Book)
        {
            if (Book == null || Book.IDL != id)
            {
                return BadRequest();
            }

            var existingBook = db.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = Book.Title;
            existingBook.Author= Book.Author;
            existingBook.YearPublication = Book.YearPublication;
            existingBook.NumberPage = Book.NumberPage;
            existingBook.Stock = Book.Stock;

            db.UpdateBook(existingBook);

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteBook")]
        public IActionResult Delete(int id)
        {
            if (db.DeleteBook(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
