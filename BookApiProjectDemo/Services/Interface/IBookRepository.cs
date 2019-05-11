using BookApiProjectDemo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface IBookRepository
    {
        ICollection<BookDto> GetAllBooks();
        BookDto GetBook(int Id);
        BookDto GetBook(string bookIsbn);

        bool BookExist(int Id);
        bool BookExist(string bookisbn);

        bool IsDuplicateIsbn(int bookId,string isbn);

        decimal GetBookRating(int bookId);



    }
}
