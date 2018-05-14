using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface ICategoryService
    {
        IEnumerable<CategoryModel> GetCategories();
        CategoryModel GetById(int id);
        CategoryModel PostCategory(CategoryModel category);
        CategoryModel PutCategory(int id, CategoryModel category);
        CategoryModel DeleteCategory(int id);


    }
}
