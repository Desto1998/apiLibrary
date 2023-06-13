using ApiLibrary.Models;
using System.Data;
using Microsoft.Data.Sqlite;
using static System.Reflection.Metadata.BlobBuilder;
using System.Net;

namespace ApiLibrary.DatabaseClasses
{
    public class DbManager
    {
        private readonly string connectionString;

        public DbManager(string connectionString)
        {
            this.connectionString = connectionString;
        }


        #region Books

        // Select all existing books in the database

        public List<Book> GetBooks()
        {
            List<Book> Books = new List<Book>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM Books", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Books.Add(CreateABook(reader));
                    }
                }
            }

            return Books;
        }

        public Book? GetBookById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Books WHERE IDL=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateABook(reader);
                        }
                    }
                }

               
            }
            return null;
        }




        private Book CreateABook(SqliteDataReader reader)
        {
            return new Book
            {
                IDL = Convert.ToInt32(reader["IDL"]),
                Title = Convert.ToString(reader["Title"]),
                Author = Convert.ToString(reader["Author"]),
                YearPublication = Convert.ToString(reader["YearPublication"]),
                NumberPage = Convert.ToInt32(reader["NumberPage"]),
                Stock = Convert.ToInt32(reader["Stock"])
            };
        }

        public int AddBook(Book Book)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO Books (Title, Author, YearPublication, NumberPage, Stock) "
                                                        + "VALUES (@title, @author, @yearP, @numPage, @stock); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@title", Book.Title);
                    command.Parameters.AddWithValue("@author", Book.Author);
                    command.Parameters.AddWithValue("@yearP", Book.YearPublication);
                    command.Parameters.AddWithValue("@numPage", Book.NumberPage);
                    command.Parameters.AddWithValue("@stock", Book.Stock);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new Book ID.");
                    }
                }
            }
        }

        public bool UpdateBook(Book Book)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Books SET Title=@title, Author=@author, YearPublication=@year, NumberPage=@numPage, Stock=@stock WHERE IDL=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", Book.IDL);
                    command.Parameters.AddWithValue("@title", Book.Title);
                    command.Parameters.AddWithValue("@author", Book.Author);
                    command.Parameters.AddWithValue("@year", Book.YearPublication);
                    command.Parameters.AddWithValue("@numPage", Book.NumberPage);
                    command.Parameters.AddWithValue("@stock", Book.Stock);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteBook(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM Books WHERE IDL=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        //  Select a book by its title
        public List<Book> GetBooksByTitle(string title)
        {
            List<Book> books = new List<Book>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Books WHERE Title=@title", conn))
                {
                    command.Parameters.AddWithValue("@title", title);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(CreateABook(reader));
                        }
                    }
                }
            }

            return books;
        }

        // Select all books who contains a given word
        public List<Book> GetBooksByKeyword(string keyword)
        {
            List<Book> books = new List<Book>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Books WHERE Title LIKE '%' || @keyword || '%'", conn))
                {
                    command.Parameters.AddWithValue("@keyword", keyword);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(CreateABook(reader));
                        }
                    }
                }
            }

            return books;
        }


        // Select all the books of the same author
        public List<Book> GetBooksByAuthor(string author)
        {
            List<Book> books = new List<Book>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Books WHERE Author=@author", conn))
                {
                    command.Parameters.AddWithValue("@author", author);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(CreateABook(reader));
                        }
                    }
                }
            }

            return books;
        }



        #endregion


        #region Borrower

        public List<Borrower> GetBorrowers()
        {
            List<Borrower> Borrowers = new List<Borrower>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM Borrower", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Borrowers.Add(CreateABorrower(reader));
                    }
                }
            }

            return Borrowers;
        }

        public Borrower? GetBorroweById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT *  FROM Borrower  WHERE BorrowedId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateABorrower(reader);
                        }
                    }
                }


            }
            return null;
        }
        #region Address
        public Address? GetAddressById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Address WHERE AddressId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateAAdress(reader);
                        }
                    }
                }


            }
            return null;
        }


        private Address CreateAAdress(SqliteDataReader reader)
        {
            return new Address
            {
                AddressId = Convert.ToInt32(reader["AddressId"]),
                Number = Convert.ToInt32(reader["Number"]),
                Street = Convert.ToString(reader["Street"]),
                ZipCode = Convert.ToString(reader["ZipCode"]),
                Country = Convert.ToString(reader["Country"]),
                Land = Convert.ToString(reader["Land"])
            };
        }

        public int AddAddress(Address address)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("INSERT INTO Address (Number, Street, ZipCode, Country, Land) "
                                                        + "VALUES (@Number, @Street, @ZipCode, @Country, @Land); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@Number", address.Number);
                    command.Parameters.AddWithValue("@Street", address.Street);
                    command.Parameters.AddWithValue("@ZipCode", address.ZipCode);
                    command.Parameters.AddWithValue("@Country", address.Country);
                    command.Parameters.AddWithValue("@Land", address.Land);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        conn.Close();
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new Address ID.");
                    }
                }
            }
        }

        public bool UpdateAddress(Address address)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Address SET Number=@Number, Street=@Street, ZipCode=@ZipCode, Country=@Country, Land=@Land WHERE AddressId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", address.AddressId);
                    command.Parameters.AddWithValue("@Number", address.Number);
                    command.Parameters.AddWithValue("@Street", address.Street);
                    command.Parameters.AddWithValue("@ZipCode", address.ZipCode);
                    command.Parameters.AddWithValue("@Country", address.Country);
                    command.Parameters.AddWithValue("@Land", address.Land);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        #endregion

        private Borrower CreateABorrower(SqliteDataReader reader)
        {
            // int addressId = Convert.ToInt32(reader["BorrewerId"]);
            return new Borrower
            {
                BorrowerId = Convert.ToInt32(reader["BorrowedId"]),
                LastName = Convert.ToString(reader["LastName"]),
                FirstName = Convert.ToString(reader["FirstName"]),
                PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                Email = Convert.ToString(reader["Email"]),
                AddressId = Convert.ToInt32(reader["AddressId"]),
                Address = GetAddressById(Convert.ToInt32(reader["AddressId"]))
                
            };
        }


 

        public int AddBorrower(Borrower borrower)
        {
            Address address = new Address();
            address.Number = borrower.Address.Number;
            address.Street = borrower.Address.Street;
            address.ZipCode = borrower.Address.ZipCode;
            address.Country = borrower.Address.Country;
            address.Land = borrower.Address.Land;
            int addressId = AddAddress(address);
            if (addressId>0){
                borrower.Address.AddressId = addressId;
                borrower.AddressId = addressId;
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    using (var command = new SqliteCommand("INSERT INTO Borrower (FirstName, LastName, AddressId, PhoneNumber, Email) "
                                                            + "VALUES (@FirstName, @LastName, @AddressId, @PhoneNumber, @Email); SELECT last_insert_rowid();", conn))
                    {
                        command.Parameters.AddWithValue("@FirstName", borrower.FirstName);
                        command.Parameters.AddWithValue("@LastName", borrower.LastName);
                        command.Parameters.AddWithValue("@AddressId", borrower.AddressId);
                        command.Parameters.AddWithValue("@PhoneNumber", borrower.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", borrower.Email);

                        var result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int newId))
                        {
                            return newId;
                        }
                        else
                        {
                            throw new Exception("Failed to retrieve the new Borrower ID.");
                        }
                    }
                }
            }
            return 0;
           
        }


       

        public bool UpdateBorrower(Borrower borrower)
        {
            UpdateAddress(borrower.Address);
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Books SET FirstName=@FirstName, LastName=@LastName, PhoneNumber=@PhoneNumber, Email=@Email WHERE BorrowedId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", borrower.BorrowerId);
                    command.Parameters.AddWithValue("@FirstName", borrower.FirstName);
                    command.Parameters.AddWithValue("@LastName", borrower.LastName);
                    command.Parameters.AddWithValue("@PhoneNumber", borrower.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", borrower.Email);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteBorrower(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM borrower WHERE borrowedId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }


        public List<Borrower> GetByNamesOrAddress(string word)
        {
            List<Borrower> borrowers = new List<Borrower>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Borrower INNER JOIN Address ON Borrower.AddressId= Address.AdressId WHERE Address.Street =@word OR Borrower.LastName=@word OR Borrower.FirstName=@word ", conn))
                {
                    command.Parameters.AddWithValue("@word", word);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            borrowers.Add(CreateABorrower(reader));
                        }
                    }
                }
            }

            return borrowers;
        }


        public List<Borrower> GetBorrowersByKeyword(string keyword)
        {
            List<Borrower> borrowers = new List<Borrower>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Borrower WHERE FirstName LIKE '%' || @keyword || '%' OR LastName LIKE '%' || @keyword || '%'", conn))
                {
                    command.Parameters.AddWithValue("@keyword", keyword);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            borrowers.Add(CreateABorrower(reader));
                        }
                    }
                }
            }

            return borrowers;
        }


        public List<Borrower> GetBorrowerOfAddress(string address)
        {
            List<Borrower> borrower = new List<Borrower>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT * FROM Borrower INNER JOIN Address ON Address.AddressId = Borrower.AddressId WHERE Address.Land=@address", conn))
                {
                    command.Parameters.AddWithValue("@address", address);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            borrower.Add(CreateABorrower(reader));
                        }
                    }
                }
            }

            return borrower;
        }


        //  Select a book by its title
        public List<Book> GetBorrowedBooks(int borrowerId)
        {
            List<Book> books = new List<Book>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
              
                using (var command = new SqliteCommand("SELECT * FROM Books INNER JOIN loan on loan.IDL = Books.IDL WHERE loan.BorrowerId=@borrowerId", conn))
                {
                    command.Parameters.AddWithValue("@borrowerId", borrowerId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(CreateABook(reader));
                        }
                    }
                }
            }

            return books;
        }
        
        public List<Borrower> GetBookBorrower(int bookId)
        {
            List<Borrower> borrowers = new List<Borrower>();

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
              
                using (var command = new SqliteCommand("SELECT * FROM borrower INNER JOIN loan ON loan.BorrowerId = borrower.BorrowedId WHERE loan.IDL=@bookId", conn))
                {
                    command.Parameters.AddWithValue("@bookId", bookId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            borrowers.Add(CreateABorrower(reader));
                        }
                    }
                }
            }

            return borrowers;
        }

        #endregion


        #region Loans


        public List<Loan> GetLoans()
        {
            List<Loan> loans = new List<Loan>();

            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using SqliteCommand command = new SqliteCommand("SELECT * FROM Loan", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loans.Add(CreateALoan(reader));
                    }
                }
            }

            return loans;
        }



        public Boolean? returBook(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Loan SET IsRetuned=1 WHERE LoanId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                   

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }


            }
            return null;
        }
       

        private Loan CreateALoan(SqliteDataReader reader)
        {
            // int addressId = Convert.ToInt32(reader["BorrewerId"]);
            return new Loan
            {
                LoanId = Convert.ToInt32(reader["LoanId"]),
                BorrowedDate = Convert.ToString(reader["BorrowedDate"]),
                ReturnDate = Convert.ToString(reader["ReturnDate"]),
                IsRetuned = Convert.ToInt32(reader["IsRetuned"]),
                BorrowerId = Convert.ToInt32(reader["BorrowerId"]),
                IDL = Convert.ToInt32(reader["IDL"]),
                Book = GetBookById(Convert.ToInt32(reader["IDL"])),
                Borrower = GetBorroweById(Convert.ToInt32(reader["BorrowerId"]))

            };
        }


        public Loan? GetLoanById(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("SELECT *  FROM Loan  WHERE LoanId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateALoan(reader);
                        }
                    }
                }


            }
            return null;
        }

        public int AddLoan(Loan loan)
        {
           
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                loan.IsRetuned = 0;
                using (var command = new SqliteCommand("INSERT INTO Loan (BorrowedDate, ReturnDate, BorrowerId, IDL, IsRetuned) "
                                                        + "VALUES (@BorrowedDate, @ReturnDate, @BorrowerId, @IDL, @IsRetuned); SELECT last_insert_rowid();", conn))
                {
                    command.Parameters.AddWithValue("@BorrowedDate", loan.BorrowedDate);
                    command.Parameters.AddWithValue("@ReturnDate", loan.ReturnDate);
                    command.Parameters.AddWithValue("@BorrowerId", loan.BorrowerId);
                    command.Parameters.AddWithValue("@IDL", loan.IDL);
                    command.Parameters.AddWithValue("@IsRetuned", loan.IsRetuned);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newId))
                    {
                        return newId;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve the new Loan ID.");
                    }
                }

            }

            return 0;

        }




        public bool UpdateLoan(Loan loan)
        {
           
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("UPDATE Loan SET BorrowedDate=@BorrowedDate, ReturnDate=@ReturnDate, BorrowerId=@BorrowerId, IDL=@IDL WHERE LoanId=@id", conn))
                {
                   
                    command.Parameters.AddWithValue("@id", loan.LoanId);
                    command.Parameters.AddWithValue("@BorrowedDate", loan.BorrowedDate);
                    command.Parameters.AddWithValue("@ReturnDate", loan.ReturnDate);
                    command.Parameters.AddWithValue("@BorrowerId", loan.BorrowerId);
                    command.Parameters.AddWithValue("@IDL", loan.IDL);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }

        public bool DeleteLoan(int id)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var command = new SqliteCommand("DELETE FROM Loan WHERE LoanId=@id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);

                    if (command.ExecuteNonQuery() >= 1) return true;
                    else return false;
                }
            }
        }


        //public List<Borrower> GetBooksByDate(string word)
        //{
        //    List<Borrower> borrowers = new List<Borrower>();

        //    using (var conn = new SqliteConnection(connectionString))
        //    {
        //        conn.Open();

        //        using (var command = new SqliteCommand("SELECT * FROM Borrower INNER JOIN Address ON Borrower.AddressId= Address.AdressId WHERE Address.Street =@word OR Borrower.LastName=@word OR Borrower.FirstName=@word ", conn))
        //        {
        //            command.Parameters.AddWithValue("@word", word);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    borrowers.Add(CreateABorrower(reader));
        //                }
        //            }
        //        }
        //    }

        //    return borrowers;
        //}


        //public List<Borrower> GetBooksBtweenDate(string keyword)
        //{
        //    List<Borrower> borrowers = new List<Borrower>();

        //    using (var conn = new SqliteConnection(connectionString))
        //    {
        //        conn.Open();

        //        using (var command = new SqliteCommand("SELECT * FROM Borrower WHERE FirstName LIKE '%' || @keyword || '%' OR LastName LIKE '%' || @keyword || '%'", conn))
        //        {
        //            command.Parameters.AddWithValue("@keyword", keyword);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    borrowers.Add(CreateABorrower(reader));
        //                }
        //            }
        //        }
        //    }

        //    return borrowers;
        //}


        //public List<Borrower> GetOutOdDateBooks(string address)
        //{
        //    List<Borrower> borrower = new List<Borrower>();

        //    using (var conn = new SqliteConnection(connectionString))
        //    {
        //        conn.Open();

        //        using (var command = new SqliteCommand("SELECT * FROM Borrower INNER JOIN Address ON Address.AddressId = Borrower.AddressId WHERE Address.Land=@address", conn))
        //        {
        //            command.Parameters.AddWithValue("@address", address);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    borrower.Add(CreateABorrower(reader));
        //                }
        //            }
        //        }
        //    }

        //    return borrower;
        //}


        ////  Select a book by its title
        //public List<Book> GetBooksByBoorower(int borrowerId)
        //{
        //    List<Book> books = new List<Book>();

        //    using (var conn = new SqliteConnection(connectionString))
        //    {
        //        conn.Open();

        //        using (var command = new SqliteCommand("SELECT * FROM Books INNER JOIN loan on loan.BookId = Books.IDL WHERE loan.BorrowerId=@borrowerId", conn))
        //        {
        //            command.Parameters.AddWithValue("@borrowerId", borrowerId);

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    books.Add(CreateABook(reader));
        //                }
        //            }
        //        }
        //    }

        //    return books;
        //}

        #endregion


    }
}
