using Project.Models;
using Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;


namespace Project.Services
{
    public class CategoryService: ICategoryService
    {
        private IUnitOfWork db;
        public CategoryService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<CategoryModel> GetCategories()
        {
            return db.CategoryModelRepository.Get();
        }
        
        public CategoryModel GetById(int id)
        {
            return db.CategoryModelRepository.GetByID(id);
           
        }
        
        public CategoryModel PostCategory(CategoryModel category)
        {
            db.CategoryModelRepository.Insert(category);
            db.Save();

            return category;
        }
        public CategoryModel PutCategory(int id, CategoryModel category)
        {
            db.CategoryModelRepository.Update(category);
            db.Save();

            return category;
        }
        public CategoryModel DeleteCategory(int id)
        {
            CategoryModel category = db.CategoryModelRepository.GetByID(id);
            if (category == null)
            {
                return null;
            }
            foreach (OfferModel nEoffer in category.Offers)
            {
                if (nEoffer.OfferExpires > DateTime.Now)
                {
                    return null;
                }
                foreach (VoucherModel nEvoucher in nEoffer.Vouchers)
                {
                    if (nEvoucher.ExpirationDate > DateTime.Now)
                    {
                        return null;
                    }
                }
            }
            db.CategoryModelRepository.Delete(id);
            db.Save();
            return category;
        }
    }
}