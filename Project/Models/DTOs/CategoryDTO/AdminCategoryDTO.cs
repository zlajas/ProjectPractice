using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.CategoryDTO
{
    public class AdminCategoryDTO : PublicCategoryDTO
    {
        public AdminCategoryDTO()
        { }
        public AdminCategoryDTO(CategoryModel category) : base(category)
        { }
    }
}