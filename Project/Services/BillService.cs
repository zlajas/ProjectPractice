using Project.Models;
using Project.Models.DTOs.Reports;
using Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Services
{
    public class BillService: IBillService
    {
        private IUnitOfWork db;
        public BillService (IUnitOfWork db)
        {
            this.db = db;
        }
        public IEnumerable<BillModel> GetBills()
        {
            return db.BillModelRepository.Get();
        }
        public BillModel GetById(int id)
        {
            return db.BillModelRepository.GetByID(id);
            
        }
        public IEnumerable<BillModel> GetByBuyers(int buyerId)
        {
            return db.BillModelRepository.Get(x => x.User.Id == buyerId);
        }
        public IEnumerable<BillModel> GetByCategory(int categoryId)
        {
            return db.BillModelRepository.Get(x => x.Offer.Category.Id == categoryId);
        }
        public IEnumerable<BillModel> GetByDateCreatedRange(DateTime startDate, DateTime endDate)
        {
            return db.BillModelRepository.Get(x => (x.BillCreated >= startDate && x.BillCreated <= endDate));

        }

        public IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate) // method to create a list of DateTime's that returns day-by-day
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                yield return startDate; // returning day-by-day   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield
                startDate = startDate.AddDays(1);
            }
        }

        public ReportDTO GetReportsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<ReportItemDTO> dailyReports = new List<ReportItemDTO>();
            ReportDTO report = new ReportDTO();
            IEnumerable<DateTime> dates = GetDateRange(startDate, endDate); 

            double income = 0;
            int numberOfOffers = 0;
            
            foreach(DateTime date in dates)
            {
                foreach(BillModel bill in GetByDateCreatedRange(startDate, endDate)) //using previously created method to loop through a list of created bills in a given date range.
                {
                    if(date == bill.BillCreated)
                    {                     
                        dailyReports.Select(x => x.ReportDate = date);
                        income += bill.Offer.ActionPrice;  //using "+=" because there might be multiple bought offers in one day.
                        
                    }                  
                }
                dailyReports.Add(new ReportItemDTO(date, income, numberOfOffers)); //adding report for one day.
                income = 0; //must reset it after each daily report
                numberOfOffers = 0;
            }
            report.Reports = dailyReports;
            report.SumOfIncomes = dailyReports.Sum(x => x.Income);
            return report;
        }

        public ReportDTO GetReportsByCategory(DateTime startDate, DateTime endDate, int categoryId)
        {
            List<ReportItemDTO> dailyReports = new List<ReportItemDTO>();
            ReportDTO report = new ReportDTO();
            IEnumerable<DateTime> dates = GetDateRange(startDate, endDate);

            report.CategoryName = db.CategoryModelRepository.GetByID(categoryId).CategoryName;

            double income = 0;
            int numberOfOffers = 0;

            foreach (var date in dates)
            {
                foreach (var bill in GetByDateCreatedRange(startDate, endDate))
                {
                    if (date == bill.BillCreated && report.CategoryName == bill.Offer.Category.CategoryName)
                    {
                        dailyReports.Select(x => x.ReportDate = date);
                        income += bill.Offer.ActionPrice;
                        numberOfOffers++;
                    }
                }
          
                dailyReports.Add(new ReportItemDTO(date, income, numberOfOffers)); 
                income = 0;  
                numberOfOffers = 0;
            }
            
            foreach (var dayReport in dailyReports)
            {
                dayReport.SerializeSensitiveInfo = true; //we now want to serialize numberOfOffers
            }

            report.SerializeSensitiveInfo = true; // we now want to serialize totalNumberOfSoldOffers and categoryName
            report.Reports = dailyReports;
            report.SumOfIncomes = dailyReports.Sum(x => x.Income);
            report.TotalNumberOfSoldOffers = dailyReports.Sum(x => x.NumberOfOffers);
            return report;
        }
        public BillModel PostBill(BillModel bill)
        {
            bill.PaymentMade = false;
            bill.PaymentCanceled = false;

            db.BillModelRepository.Insert(bill);
            db.Save();

            return bill;

        }
        public BillModel PutBill(int id, BillModel bill)
        {
            BillModel updatedBill = db.BillModelRepository.GetByID(id);

            if (updatedBill != null)
            {
                updatedBill.PaymentMade = bill.PaymentMade;
                updatedBill.PaymentCanceled = bill.PaymentCanceled;
                updatedBill.Offer = bill.Offer;
                updatedBill.User = bill.User;

                db.BillModelRepository.Update(bill);
                db.Save();
            }
            return bill;
        }
        public BillModel DeleteBill(int id)
        {
            BillModel bill = db.BillModelRepository.GetByID(id);
            if (bill == null)
            {
                return null;
            }
            db.BillModelRepository.Delete(bill);
            db.Save();
            return bill;
        }
      /*  public BillModel PutOfferOnBill(int id, int offerId)
        {
            BillModel bill = db.BillModelRepository.GetByID(id);
            if (bill == null)
            {
                return null;
            }
            OfferModel offer = db.OfferModelRepository.GetByID(offerId);
            if (offer == null)
            {
                return null;
            }
            bill.Offer = offer;
            db.BillModelRepository.Update(bill);
            db.Save();
            return bill;
        }
        public BillModel PutChangesToOffer(int id, BillModel updatedBill)
        {
            BillModel bill = db.BillModelRepository.GetByID(id);
            if (bill == null)
            {
                return null;
            }

            bill.Offer.OfferName = updatedBill.Offer.OfferName;
            bill.Offer.OfferDescription = updatedBill.Offer.OfferDescription;
            bill.Offer.OfferCreated = updatedBill.Offer.OfferCreated;
            bill.Offer.ImagePath = updatedBill.Offer.ImagePath;
            bill.Offer.OfferExpires = updatedBill.Offer.OfferExpires;
            bill.Offer.RegularPrice = updatedBill.Offer.RegularPrice;
            bill.Offer.ActionPrice = updatedBill.Offer.ActionPrice;
            bill.Offer.AvailableOffers = updatedBill.Offer.AvailableOffers;
            bill.Offer.BoughtOffers = updatedBill.Offer.BoughtOffers;
            db.BillModelRepository.Update(bill);
            db.Save();
            return bill;
        }
        public BillModel PutUserToBill(int id, int userId)
        {
            BillModel bill = db.BillModelRepository.GetByID(id);
            if (bill == null)
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
                bill.User = user;
                db.BillModelRepository.Update(bill);
                db.Save();
                return bill;
            }
            else
            {
                return null;
            }

        }
        public BillModel PutChangesToBillUser(int id, BillModel updateBill)
        {
            BillModel bill = db.BillModelRepository.GetByID(id);
            if (bill == null)
            {
                return null;
            }

            bill.User.FirstName = updateBill.User.FirstName;
            bill.User.LastName = updateBill.User.LastName;
            bill.User.Username = updateBill.User.Username;
            bill.User.Password = updateBill.User.Password;
            bill.User.Email = updateBill.User.Email;
            db.BillModelRepository.Update(bill);
            db.Save();
            return bill;
        }*/
        
    }
}