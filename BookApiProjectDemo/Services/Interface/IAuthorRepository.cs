using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface IAuthorRepository
    {
        ICollection<AuthorDto> GetAllAuthors();
        AuthorDto GetAuthor(int authorId);
        ICollection<AuthorDto> GetAllAuthorsOfABook(int bookId);
        ICollection<BookDto> GetAllBooksByAnAuthor(int authorId);
        bool AuthorExist(int authorId);
    }
}
