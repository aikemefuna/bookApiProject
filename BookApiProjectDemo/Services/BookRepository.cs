using AutoMapper;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BookApiDbContext _bookContext;
        private readonly IMapper _mapper;

        public BookRepository(BookApiDbContext bookContext, IMapper mapper)
        {
            _bookContext = bookContext;
            _mapper = mapper;
        }
        public bool BookExist(int bookId)
        {
           
            return _bookContext.Books.Any(b => b.Id == bookId);
        }

        public bool BookExist(string bookIsbn)
        {
            return _bookContext.Books.Any(b => b.Isbn.Equals(bookIsbn));
        }

        public ICollection<BookDto> GetAllBooks()
        {
            var books = _bookContext.Books.ToList();
            var booksQuery = _mapper.Map<IList<BookDto>>(books);

            return booksQuery;
        }

        public BookDto GetBook(int bookId)
        {
            var bookById = _bookContext.Books.SingleOrDefault(b => b.Id == bookId);
            var bookIdQuery = _mapper.Map<BookDto>(bookById);

            return bookIdQuery;

        }

        public BookDto GetBook(string bookIsbn)
        {
            var bookByIsbn = _bookContext.Books.SingleOrDefault(b => b.Isbn.Equals(bookIsbn));
            var bookIsbnQuery = _mapper.Map<BookDto>(bookByIsbn);

            return bookIsbnQuery;
        }

        public decimal GetBookRating(int bookId)
        {
            var review = _bookContext.Reviews.Where(r => r.Book.Id == bookId);
            if (review.Count() <= 0)
                return 0;
            var averageRating = ((decimal)review.Sum(r => r.Rating) / review.Count());
            return averageRating;
       }

        public bool IsDuplicateIsbn(int bookId, string isbn)
        {
            var DuplicateIsbn = _bookContext.Books.SingleOrDefault(b => b.Isbn.Trim().ToUpper().Equals(isbn.Trim().ToUpper()) && b.Id != bookId);

            return DuplicateIsbn == null ? false : true;
        }
    }
}
