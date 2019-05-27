using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface IBookRepository
    {
        ICollection<BookDto> GetAllBooks();
        Book GetBook(int Id);
        Book GetBook(string bookIsbn);

        bool BookExist(int Id);
        bool BookExist(string bookisbn);

        bool IsDuplicateIsbn(int bookId,string isbn);

        decimal GetBookRating(int bookId);


        bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book);
        bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book);
        bool DeleteBook(Book book);
        bool Save();




    }
}
