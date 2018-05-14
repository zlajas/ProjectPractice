using Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IVoucherService
    {
        IEnumerable<VoucherModel> GetVouchers();
        VoucherModel GetVoucherById(int id);
        void PostVoucher(BillModel bill);
        VoucherModel PutVoucher(int id, VoucherModel voucher);
      /*  VoucherModel PutOfferOnVoucher(int id, int offerId);
        VoucherModel PutUserToVoucher(int id, int userId);*/
        IEnumerable<VoucherModel> GetVoucherByBuyers(int buyerId);
        IEnumerable<VoucherModel> GetVoucherByOffers(int offerId);
        IEnumerable<VoucherModel> GetNonExpiredVouchers();
        VoucherModel DeleteVoucher(int id);
        

    }
}
