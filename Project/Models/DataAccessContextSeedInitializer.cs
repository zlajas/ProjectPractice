using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

//By Milan Kablar

namespace Project.Models
{
    public enum FirstNames { Milan, Nenad, Marko, Nikola, Veselin, Natasa, Doris, Nikolina, Zdenka, Danijela, Goran, Nemanja, Jovana }
    public enum LastNames { Miljanic, Stojakovic, Djordjevic, Grujic, Markovic, Petrovic, Nikolic, Jovanovic, Nenadic, Stankovic }
    public class DataAccessContextSeedInitializer : DropCreateDatabaseIfModelChanges<DataAccessContext>
    {
        public Random rnd = new Random();
        private int userCount = 15;
        private int userAdminCount = 2;
        private int userSellerCount = 4;

        private int categoryCount = 5;
        private int offerCount = 15;

        private int billCount = 20;
        private DateTime lastDate;

        protected override void Seed(DataAccessContext context)
        {
            // Add users
            UserModel[] users = new UserModel[userCount];
            for (int i = 0; i < userCount; i++)
            {
                UserModel user = new UserModel();
                user.FirstName = Enum.GetName(typeof(FirstNames), rnd.Next(Enum.GetValues(typeof(FirstNames)).Length));
                user.LastName = Enum.GetName(typeof(LastNames), rnd.Next(Enum.GetValues(typeof(LastNames)).Length));
                user.Username = string.Format("{0}{1}{2}", user.FirstName.ToLower(), user.LastName.ToLower(), i + 1);
                user.Password = "12345";
                user.Email = string.Format("{0}@example.com", user.Username);
                // next line is for all roles to be random
                user.UserRole = (UserRole)Enum.GetValues(typeof(UserRole)).GetValue(rnd.Next(Enum.GetValues(typeof(UserRole)).Length));

                if (i >= userAdminCount + userSellerCount)
                {
                    user.UserRole = UserRole.ROLE_CUSTOMER;
                }
                else if (i >= userAdminCount)
                {
                    user.UserRole = UserRole.ROLE_SELLER;
                }
                else
                {
                    user.UserRole = UserRole.ROLE_ADMIN;
                }
                context.UserModel.Add(user);
                users[i] = user;
            }
            context.SaveChanges();

            // Add categories
            CategoryModel[] categories = new CategoryModel[categoryCount];
            for (int i = 0; i < categoryCount; i++)
            {
                CategoryModel category = new CategoryModel();
                category.CategoryName = string.Format("Category{0}", i + 1);
                category.CategoryDescription = string.Format("Really good {0}", category.CategoryName);

                context.CategoryModel.Add(category);
                categories[i] = category;
            }
            context.SaveChanges();

            // Add offers
            OfferModel[] offers = new OfferModel[offerCount];
            for (int i = 0; i < offerCount; i++)
            {
                // Create an offer and fill in all the details
                OfferModel offer = new OfferModel();
                offer.OfferName = string.Format("Offer{0}", i + 1);
                offer.OfferDescription = string.Format("Excellent {0}", offer.OfferName);
                // Create offer up to 50 days in the past or 50 days in the future
                offer.OfferCreated = DateTime.Today.AddDays(rnd.Next(100) - 50);
                // Offer expires from 7-30 days from the date of creation
                offer.OfferExpires = offer.OfferCreated.AddDays(rnd.Next(23) + 7);
                // Regular price between 0 and 1000
                offer.RegularPrice = Math.Round(rnd.NextDouble() * 1000);
                // ActionPrice will be up to 50% cheaper than RegularPrice
                offer.ActionPrice = Math.Round(offer.RegularPrice * (rnd.NextDouble() / 2 + 0.5), 2);
                offer.ImagePath = string.Format("/images/offers/offer{0}.jpg", i + 1);
                offer.AvailableOffers = rnd.Next(20);

                int offerStatusLength;
                if (offer.OfferExpires < DateTime.UtcNow)
                {
                    offer.OfferStatus = OfferStatus.EXPIRED;
                }
                else
                {
                    offerStatusLength = offer.OfferCreated > DateTime.UtcNow ? 3 : 2;
                    // If the offer is not expired
                    // And it was created in the past we only need the status APPROVED or DECLINED, and they are the first two
                    // If it is yet to be created it can also be WAITING_FOR_APPROVING
                    offer.OfferStatus = (OfferStatus)Enum.GetValues(typeof(OfferStatus)).GetValue(rnd.Next(offerStatusLength));
                }

                // If the offer was declined, or still not approved, nothing could have sold by now
                if (offer.OfferStatus == OfferStatus.DECLINED || offer.OfferStatus == OfferStatus.WAIT_FOR_APPROVING)
                {
                    offer.BoughtOffers = 0;
                }
                else
                {
                    offer.BoughtOffers = rnd.Next(20);
                }
                // It must be a Seller user
                offer.User = users[rnd.Next(userSellerCount) + userAdminCount];
                offer.Category = categories[rnd.Next(categories.Length)];

                context.OfferModel.Add(offer);
                offers[i] = offer;
            }
            context.SaveChanges();

            // Add bills
            BillModel[] bills = new BillModel[billCount];
            OfferModel[] currentAndPastOffers = offers.Where(x => x.OfferCreated < DateTime.UtcNow).ToArray();
            for (int i = 0; i < billCount; i++)
            {
                BillModel bill = new BillModel();
                bill.PaymentMade = rnd.Next(2) == 1;
                // Bill can only happen up until now
                bill.Offer = currentAndPastOffers[rnd.Next(currentAndPastOffers.Length)];
                // If Offer is expired and bill was not payed for, we cancel the bill
                bill.PaymentCanceled = (bill.Offer.OfferStatus == OfferStatus.EXPIRED && !bill.PaymentMade) ? true : false;
                // Bill was created at a random date between when the offer is created and when it expires, or today
                lastDate = bill.Offer.OfferExpires < DateTime.UtcNow ? bill.Offer.OfferExpires : DateTime.UtcNow;
                bill.BillCreated = bill.Offer.OfferCreated.AddDays(rnd.Next((lastDate - bill.Offer.OfferCreated).Days));

                // User can only be a Customer
                bill.User = users[rnd.Next(users.Length - (userAdminCount + userSellerCount)) + userAdminCount + userSellerCount];

                context.BillModel.Add(bill);
                bills[i] = bill;
            }
            context.SaveChanges();

            // Add vouchers
            BillModel[] billsPaymentMade = bills.Where(x => x.PaymentMade == true).ToArray();
            VoucherModel[] vouchers = new VoucherModel[billsPaymentMade.Length];
            for (int i = 0; i < vouchers.Length; i++)
            {
                VoucherModel voucher = new VoucherModel();
                voucher.Offer = billsPaymentMade[i].Offer;
                voucher.User = billsPaymentMade[i].User;
                voucher.IsUsed = rnd.Next(2) == 1;
                // Voucher can be used within the next 7 days
                voucher.ExpirationDate = billsPaymentMade[i].BillCreated.AddDays(7);

                context.VoucherModel.Add(voucher);
                vouchers[i] = voucher;
            }
            context.SaveChanges();

            base.Seed(context);
        }
    }
}