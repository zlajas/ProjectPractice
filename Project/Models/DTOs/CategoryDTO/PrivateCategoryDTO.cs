using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.CategoryDTO
{
    public class PrivateCategoryDTO : PublicCategoryDTO
    {
        public PrivateCategoryDTO()
        { }
        public PrivateCategoryDTO(CategoryModel category) : base(category)
        {

        }
    }
}