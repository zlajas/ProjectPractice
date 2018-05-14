using Project.Models;
using Project.Models.DTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IBillService
    {
        IEnumerable<BillModel> GetBills();
        BillModel GetById(int id);
        IEnumerable<BillModel> GetByBuyers(int buyerId);
        IEnumerable<BillModel> GetByCategory(int categoryId);
        IEnumerable<BillModel> GetByDateCreatedRange(DateTime startDate, DateTime endDate);
        BillModel PostBill(BillModel bill);
        BillModel PutBill(int id, BillModel bill);
        BillModel DeleteBill(int id);
        ReportDTO GetReportsByDateRange(DateTime startDate, DateTime endDate);
        ReportDTO GetReportsByCategory(DateTime startDate, DateTime endDate, int categoryId);


       /* BillModel PutOfferOnBill(int id, int offerId);
        BillModel PutChangesToOffer(int id, BillModel updatedBill);
        BillModel PutUserToBill(int id, int userId);
        BillModel PutChangesToBillUser(int id, BillModel updateBill);*/



    }
}
