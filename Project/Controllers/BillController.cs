using Project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Project.Services;
using Project.Models.DTOs.BillDTO;
using Project.Models.DTOs.Reports;

namespace Project.Controllers
{
    [RoutePrefix("project/bills")]
    public class BillController : ApiController

    {
        private IBillService billService;
        private IUsersService usersService;
        private IVoucherService voucherService;
        private IOfferService offerService;
        public BillController (IBillService billService, IUsersService usersService, IVoucherService voucherService, IOfferService offerService)
        {
            this.billService = billService;
            this.usersService = usersService;
            this.voucherService = voucherService;
            this.offerService = offerService;
        }
        [Route("public")]
        public IEnumerable<PublicBillDTO> GetPublicBillsView()
        {
            return billService.GetBills().Select(x=> new PublicBillDTO(x));
        }

        [Route("private")]
        public IEnumerable<PrivateBillDTO> GetPrivateBillsView()
        {
            return billService.GetBills().Select(x => new PrivateBillDTO(x));
        }

        [Route("admin")]
        public IEnumerable<AdminBillDTO> GetAdminBillsView()
        {
            return billService.GetBills().Select(x => new AdminBillDTO(x));
        }

        [Route("generateReport/{startDate}/and/{endDate}")]
        public ReportDTO GetReportByDateRange(DateTime startDate, DateTime endDate)
        {
            return billService.GetReportsByDateRange(startDate, endDate);

        }
        [Route("generateReport/{startDate}/and/{endDate}/category/{categoryID}")]
        public ReportDTO GetReportsByCategory(DateTime startDate, DateTime endDate, int categoryId)
        {
            return billService.GetReportsByCategory(startDate, endDate, categoryId);
        }

        [Route("{id}")]
        [ResponseType(typeof(BillModel))]
        public IHttpActionResult GetBillById(int id)
        {
            BillModel bill = billService.GetById(id);
            if (bill == null)
            {
                return NotFound();
            }
            return Ok(bill);
        }
        [Route("findByBuyer/{buyerId}")]
        [ResponseType(typeof(BillModel))]
        public IHttpActionResult GetByBuyers (int buyerId)
        {
            UserModel user = usersService.GetById(buyerId);
            if(user==null)
            {
                return NotFound();
            }
            return Ok(usersService.GetById(buyerId).Bills);
        }
        [Route("findByCategory/{categoryId}")]
        public IEnumerable<BillModel> GetByCategory(int categoryId)
        {  
            return billService.GetByCategory(categoryId);
        }
        [Route("findByDate/{startDate}/and/{endDate}")]
        [ResponseType(typeof(List<BillModel>))]
        public IHttpActionResult GetByDateCreatedRange(DateTime startDate, DateTime endDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           
            return Ok(billService.GetByDateCreatedRange(startDate, endDate));
        }
        [Route("", Name ="PostBill")]
        [ResponseType(typeof(BillModel))]
        public IHttpActionResult PostBill(BillModel bill)
        {
            if (!ModelState.IsValid || bill.OfferId == null || bill.BuyerId == null)
            {
                return BadRequest(ModelState);
            }

            OfferModel offer = offerService.GetById((int)bill.OfferId);
            UserModel buyer = usersService.GetById((int)bill.BuyerId);

            if(offer == null || buyer == null)
            {
                return NotFound();
            }

            if(buyer.UserRole != UserRole.ROLE_CUSTOMER)
            {
                return BadRequest("User's role muse be ROLE_CUSTOMER");
            }

            bill.Offer = offer;
            bill.User = buyer;

            BillModel createdBill = billService.PostBill(bill);

            return CreatedAtRoute("PostBill", new { id = createdBill.Id}, createdBill);
        }
        [ResponseType(typeof(BillModel))]
        [Route("{id}")]
        public IHttpActionResult PutBill(int id, BillModel bill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != bill.Id)
            {
                return BadRequest();
            }

            BillModel updatedBill = billService.PutBill(id, bill);

            if(updatedBill == null)
            {
                return NotFound();
            }

            if(updatedBill.PaymentMade == true)
            {
                voucherService.PostVoucher(updatedBill);

            }
            if(updatedBill.PaymentCanceled)
            {
                offerService.PutOffer(updatedBill.Offer, false);
            }

            return Ok(updatedBill);
        }

        [ResponseType(typeof(BillModel))]
        [Route("{id}")]
        public IHttpActionResult DeleteBill(int id)
        {
            BillModel bill = billService.DeleteBill(id);
            if (bill == null)
            {
                return NotFound();
            }        
            return Ok(bill);
        }

      /*  [ResponseType(typeof(BillModel))]
        [Route("update-offer-on-bill/{id}")]
        public IHttpActionResult PutChangesToOffer(int id, BillModel updateBill)
        {
            BillModel bill = billService.GetById(id);
            if (bill == null)
            {
                return NotFound();
            }
            return Ok(billService.PutChangesToOffer(id, updateBill));
        }
       
        [ResponseType(typeof(BillModel))]
        [Route("api/bill/update-user-on-bill/{id}")]
        public IHttpActionResult PutChangesToBillUser(int id, BillModel updateBill)
        {
            BillModel bill = billService.GetById(id);
            if (bill == null)
            {
                return NotFound();
            }
            
            return Ok(billService.PutChangesToBillUser(id, updateBill));
        }*/
    }
}