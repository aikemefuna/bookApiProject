using AutoMapper;
using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
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

        public bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _bookContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            foreach (var author in authors)
            {
                var bookAuthors = new BookAuthor
                {
                    Author = author,
                    Book = book
                };

                _bookContext.Add(bookAuthors);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory
                {
                    Category = category,
                    Book = book
                };
                _bookContext.Add(bookCategory);
            }


            _bookContext.Add(book);

            return Save();
        }

        public bool DeleteBook(Book book)
        {
            _bookContext.Remove(book);
            return Save();
        }

        public ICollection<BookDto> GetAllBooks()
        {
            var books = _bookContext.Books.ToList();
            var booksQuery = _mapper.Map<IList<BookDto>>(books);

            return booksQuery;
        }

        public Book GetBook(int bookId)
        {
            var bookById = _bookContext.Books.SingleOrDefault(b => b.Id == bookId);
            //var bookIdQuery = _mapper.Map<BookDto>(bookById);

            return bookById;

        }

        public Book GetBook(string bookIsbn)
        {
            var bookByIsbn = _bookContext.Books.SingleOrDefault(b => b.Isbn.Equals(bookIsbn));
           // var bookIsbnQuery = _mapper.Map<BookDto>(bookByIsbn);

            return bookByIsbn;
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

        public bool Save()
        {
            var isSaved = _bookContext.SaveChanges();
            return isSaved > 0 ? true : false;
        }

        public bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _bookContext.Authors.Where(a => authorsId.Contains(a.Id)).ToList();
            var categories = _bookContext.Categories.Where(c => categoriesId.Contains(c.Id)).ToList();

            var booksAuthorToDelete = _bookContext.BookAuthors.Where(b => b.BookId == book.Id);
            var booksCategoriesToDelete = _bookContext.BookCategories.Where(b => b.BookId == book.Id);

            _bookContext.RemoveRange(booksAuthorToDelete);
            _bookContext.RemoveRange(booksCategoriesToDelete);

            foreach (var  author in authors)
            {
                var bookAuthor = new BookAuthor
                {
                     Author = author,
                      Book = book
                };
                _bookContext.Add(bookAuthor);
            }

            foreach (var category in categories)
            {
                var bookCategory = new BookCategory
                {
                    Category = category,
                    Book = book
                };
                _bookContext.Add(bookCategory);

            }

            _bookContext.Update(book);

            return Save();

        }
    }
}
