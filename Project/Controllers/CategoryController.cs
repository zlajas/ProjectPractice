using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Project.Models;
using Project.Repositories;
using Project.Services;
using Project.Models.DTOs.CategoryDTO;

namespace Project.Controllers
{
    [RoutePrefix("project/categories")]
    public class CategoryController : ApiController
    {
        private ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [Route("public")]
        public IEnumerable<PublicCategoryDTO> GetPublicCategoriesView()
        {
            return categoryService.GetCategories().Select(x => new PublicCategoryDTO(x));
        }

        [Route("private")]
        public IEnumerable<PrivateCategoryDTO> GetPrivateCategoriesView()
        {
            return categoryService.GetCategories().Select(x => new PrivateCategoryDTO(x));
        }

        [Route("admin")]
        public IEnumerable<AdminCategoryDTO> GetAdminCategoriesView()
        {
            return categoryService.GetCategories().Select(x => new AdminCategoryDTO(x));
        }

        [Route("api/category/{id}")]
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult GetCategory(int id)
        {
            CategoryModel category = categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        [Route("", Name ="PostCategory")]
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult PostCategory(CategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CategoryModel createdCategory = categoryService.PostCategory(category);

            return CreatedAtRoute("PostCategory", new { id = createdCategory.Id }, createdCategory);
        }

        [Route("{id}")]
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult PutCategory(int id, CategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }
            CategoryModel updatedCategory = categoryService.PutCategory(id, category);

            if(updatedCategory == null)
            {
                return NotFound();
            }
            return Ok(updatedCategory);
        }

        [Route("{id}")]
        [ResponseType(typeof(CategoryModel))]
        public IHttpActionResult DeleteCategory(int id)
        {
            CategoryModel category = categoryService.DeleteCategory(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
    }
       
    
}
