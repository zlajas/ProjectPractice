using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using Project.Models.DTOs;

namespace Project.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepository<UserModel> UserModelRepository { get; }
        IGenericRepository<CategoryModel> CategoryModelRepository { get; }
        IGenericRepository<OfferModel> OfferModelRepository { get; }
        IGenericRepository<BillModel> BillModelRepository { get; }
        IGenericRepository<VoucherModel> VoucherModelRepository { get; }
        void Save();
    }
}
