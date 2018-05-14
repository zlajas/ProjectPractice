using Project.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class DataAccessContext : DbContext
    {
       
        public DataAccessContext() :
            base("ProjectConnection")
        {
            Database.SetInitializer(new DataAccessContextSeedInitializer());
        }
        public DbSet<UserModel> UserModel { get; set; }
        public DbSet<CategoryModel> CategoryModel { get; set; }
        public DbSet<OfferModel> OfferModel { get; set; }
        public DbSet<BillModel> BillModel { get; set; }
        public DbSet<VoucherModel> VoucherModel { get; set; }

    }
}