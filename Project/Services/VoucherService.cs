using Project.Models;
using Project.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Diagnostics;

namespace Project.Services
{
    public class VoucherService: IVoucherService
    {
        private IUnitOfWork db;
        
        public VoucherService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<VoucherModel> GetVouchers()
        {
            return db.VoucherModelRepository.Get();
        }

        public VoucherModel GetVoucherById(int id)
        {
           return db.VoucherModelRepository.GetByID(id);
            
        }
        public void PostVoucher(BillModel bill)
        {
            VoucherModel voucher = new VoucherModel()
            {
                User = bill.User,
                Offer = bill.Offer,
                IsUsed = false,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };
        }
        public VoucherModel PutVoucher(int id, VoucherModel voucher)
        {
            VoucherModel updateVoucher = db.VoucherModelRepository.GetByID(id);

            if (updateVoucher != null)
            {
                updateVoucher.ExpirationDate = voucher.ExpirationDate;
                updateVoucher.IsUsed = voucher.IsUsed;
                updateVoucher.Offer = voucher.Offer;
                updateVoucher.User = voucher.User;

                db.VoucherModelRepository.Update(updateVoucher);
                db.Save();
            }

            return updateVoucher;
        }
       /* public VoucherModel PutOfferOnVoucher(int id, int offerId)
        {
            VoucherModel voucher = db.VoucherModelRepository.GetByID(id);
            if (voucher == null)
            {
                return null;
            }
            OfferModel offer = db.OfferModelRepository.GetByID(offerId);
            if (offer == null)
            {
                return null;
            }
            voucher.Offer = offer;
            db.VoucherModelRepository.Update(voucher);
            db.Save();
            return voucher;
        }
        public VoucherModel PutUserToVoucher(int id, int userId)
        {
            VoucherModel voucher = db.VoucherModelRepository.GetByID(id);
            if (voucher == null)
            {
                return null;
            }
            UserModel user = db.UserModelRepository.GetByID(userId);
            if (user == null)
            {
                return null;
            }
            if ((int)user.UserRole == 1)
            {
                voucher.User = user;
                db.VoucherModelRepository.Update(voucher);
                db.Save();
                return voucher;
            }
            else
            {
                return null;
            }

        }*/
        public IEnumerable<VoucherModel> GetVoucherByBuyers(int buyerId)
        {
            return db.VoucherModelRepository.Get(x => x.User.Id == buyerId);
        }
        public IEnumerable<VoucherModel> GetVoucherByOffers(int offerId)
        {
            return db.VoucherModelRepository.Get(x => x.Offer.Id == offerId);
        }
        public IEnumerable<VoucherModel> GetNonExpiredVouchers()
        {
            return db.VoucherModelRepository.Get(x => x.ExpirationDate < DateTime.UtcNow);
        }
        public VoucherModel DeleteVoucher(int id)
        {
            VoucherModel voucher = db.VoucherModelRepository.GetByID(id);
            if (voucher == null)
            {
                return null;
            }
            db.VoucherModelRepository.Delete(id);
            db.Save();
            
            return voucher;
        }
        public VoucherModel PostVoucherOnPaidBill(int billId, int userId, int offerId, VoucherModel voucher)
        {
            BillModel bill = db.BillModelRepository.GetByID(billId);
            if (bill.PaymentMade == true)
            {
                db.VoucherModelRepository.Insert(voucher);
            }
            return voucher;

        }
       
    }
}
