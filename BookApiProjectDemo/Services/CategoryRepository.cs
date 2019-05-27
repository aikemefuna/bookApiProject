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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookApiDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(BookApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<CategoryDto> GetCategories()
        {
            var categories = _context.Categories.ToList();
            var categoriesQuery = _mapper.Map<ICollection<CategoryDto>>(categories);
            return categoriesQuery;
        }

        public Category GetCategory(int categoryId)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Id == categoryId);
          //  var categoryQuery = _mapper.Map<CategoryDto>(category);
            return category;
        }
        public ICollection<BookDto> GetAllBooksOfACategory(int categoryId)
        {
            var booksCategory = _context.BookCategories.Where(c => c.CategoryId == categoryId).Select(b => b.Book).ToList();
            var booksCategoryQuery = _mapper.Map<IList<BookDto>>(booksCategory);

            return booksCategoryQuery;
        }

        public ICollection<CategoryDto> GetAllCategoriesOfABook(int bookId)
        {
            var categories = _context.BookCategories.Where(b => b.BookId == bookId).Select(c => c.Category).ToList();
           var categoryQuery = _mapper.Map<ICollection<CategoryDto>>(categories);

            return categoryQuery;
        }

        public bool CategoryExist(int categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool IsDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _context.Categories.SingleOrDefault(c => c.Name.Equals(categoryName) && c.Id != categoryId);
            return category == null ? false : true;
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
            return Save();
        }

        public bool Save()
        {
            var IsSaved = _context.SaveChanges();
            return IsSaved >=0 ? true : false;
        }
    }
}
