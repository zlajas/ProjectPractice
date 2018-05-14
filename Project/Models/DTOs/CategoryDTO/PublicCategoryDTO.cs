using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.CategoryDTO
{
    public class PublicCategoryDTO
    {
        public PublicCategoryDTO()
        { }
        
        public PublicCategoryDTO(CategoryModel category)
        {
            Id = category.Id;
            CategoryName = category.CategoryName;
            CategoryDescription = category.CategoryDescription;
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

    }
}