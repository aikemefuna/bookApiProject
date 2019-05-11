using BookApiProjectDemo.DTO;
using BookApiProjectDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiProjectDemo.Services.Interface
{
    public interface ICategoryRepository
    {
        ICollection<CategoryDto> GetCategories();
        CategoryDto GetCategory(int categoryId);
        ICollection<BookDto> GetAllBooksOfACategory(int categoryId);
        ICollection<CategoryDto> GetAllCategoriesOfABook(int bookId);
        bool CategoryExist(int categoryId);
        bool IsDuplicateCategoryName(int categoryId, string categoryName);
    }
}
