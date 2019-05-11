using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Services.Interface;

namespace BookApiProjectDemo.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookApiDbContext _authorContext;
        private readonly IMapper _mapper;

        public AuthorRepository(BookApiDbContext authorContext, IMapper mapper)
        {
            _authorContext = authorContext;
            _mapper = mapper;
        }
        public bool AuthorExist(int authorId)
        {
            return _authorContext.Authors.Any(a => a.Id == authorId);
        }

        public ICollection<AuthorDto> GetAllAuthors()
        {
            var authors = _authorContext.Authors.OrderBy(a => a.FirstName).ToList();
            var authorsQuery = _mapper.Map<IList<AuthorDto>>(authors);

            return authorsQuery;
        }

        public ICollection<AuthorDto> GetAllAuthorsOfABook(int bookId)
        {
            var authors = _authorContext.BookAuthors.Where(b => b.BookId == bookId).Select(a => a.Author).ToList();
            var authorsQuery = _mapper.Map<ICollection<AuthorDto>>(authors);

            return authorsQuery;
        }

        public ICollection<BookDto> GetAllBooksByAnAuthor(int authorId)
        {
            var books = _authorContext.BookAuthors.Where(a => a.AuthorId == authorId).Select(b => b.Book).ToList();
            var booksQuery = _mapper.Map<ICollection<BookDto>>(books);

            return booksQuery;
        }

        public AuthorDto GetAuthor(int authorId)
        {
            var author = _authorContext.Authors.SingleOrDefault(a => a.Id == authorId);
            var authorQuery = _mapper.Map<AuthorDto>(author);

            return authorQuery;
        }
    }
}
