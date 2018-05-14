
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Unity.Attributes;
using System;
using Project.Repositories;
using Project.Models;
using Project.Models.DTOs;

namespace Project.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private DbContext context;
        public UnitOfWork(DbContext context)
        {
            this.context = context;
        }

        [Dependency]
        public IGenericRepository<UserModel> UserModelRepository { get; set; }
        [Dependency]
        public IGenericRepository<CategoryModel> CategoryModelRepository { get; set; }
        [Dependency]
        public IGenericRepository<OfferModel> OfferModelRepository { get; set; }
        [Dependency]
        public IGenericRepository<BillModel> BillModelRepository { get; set; }
        [Dependency]
        public IGenericRepository<VoucherModel> VoucherModelRepository { get; set; }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}