using Project.Models;
using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Project.Repositories;
using Project.Services;
using System.IO;
using System.Net.Mail;
using Project.Models.DTOs.VoucherDTO;
using System.Linq;

namespace Project.Controllers
{
    [RoutePrefix("project/vouchers")]
    public class VoucherController : ApiController
    {
        private IVoucherService voucherService;
        private IOfferService offerService;
        private IUsersService usersService;
        public VoucherController(IVoucherService voucherService, IOfferService offerService, IUsersService usersService)
        {
            this.voucherService = voucherService;
            this.offerService = offerService;
            this.usersService = usersService;
        }
        [Route("public")]
        public IEnumerable<PublicVoucherDTO> GetPublicVouchersView()
        {
            return voucherService.GetVouchers().Select(x => new PublicVoucherDTO(x));
        }

        [Route("private")]
        public IEnumerable<PrivateVoucherDTO> GetPrivateVouchersView()
        {
            return voucherService.GetVouchers().Select(x => new PrivateVoucherDTO(x));
        }

        [Route("admin")]
        public IEnumerable<AdminVoucherDTO> GetAdminVouchersView()
        {
            return voucherService.GetVouchers().Select(x => new AdminVoucherDTO(x));
        }

        [Route("{id}")]
        [ResponseType(typeof(VoucherModel))]
        public IHttpActionResult GetVoucherById(int id)
        {
            VoucherModel voucher = voucherService.GetVoucherById(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return Ok(voucher);
        }
        /*
        [Route("", Name ="PostVoucher")]
        [ResponseType(typeof(VoucherModel))]
        public IHttpActionResult PostVoucher (VoucherModel voucher)
        {
            if(!ModelState.IsValid || voucher.OfferId == null || voucher.BuyerId == null)
            {
                return BadRequest(ModelState);
            }

            OfferModel offer = offerService.GetById((int)voucher.OfferId);
            UserModel buyer = usersService.GetById((int)voucher.BuyerId);

            if(offer == null || buyer == null)
            {
                return NotFound();
            }

            if(buyer.UserRole != UserRole.ROLE_CUSTOMER)
            {
                return BadRequest("User's role must be ROLE_CUSTOMER");
            }

            voucher.Offer = offer;
            voucher.User = buyer;
            VoucherModel createdVoucher = voucherService.PostVoucher(voucher);

            return CreatedAtRoute("PostOffer", new { id = createdVoucher.Id}, createdVoucher);
        }*/
        [Route("{id}")]
        [ResponseType(typeof(VoucherModel))]
        public IHttpActionResult PutVoucher(int id, VoucherModel voucher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != voucher.Id)
            {
                return BadRequest();
            }

            OfferModel offer = offerService.GetById((int)voucher.OfferId);
            UserModel buyer = usersService.GetById((int)voucher.BuyerId);

            if (offer == null || buyer == null)
            {
                return NotFound();
            }

            if (buyer.UserRole != UserRole.ROLE_CUSTOMER)
            {
                return BadRequest("User's role must be ROLE_CUSTOMER");
            }

            voucher.Offer = offer;
            voucher.User = buyer;
            VoucherModel updatedVoucher = voucherService.PutVoucher(id, voucher);

            if (updatedVoucher == null)
            {
                return NotFound();
            }

            return Ok(updatedVoucher);
        }
        /*
        [ResponseType(typeof(VoucherModel))]
        [Route("{id}/putOffer/{offerId}", Name = "AddOfferOnVoucher")]
        public IHttpActionResult PutOfferOnVoucher(int id, int offerId)
        {
            VoucherModel voucher = voucherService.GetVoucherById(id);
            if (voucher == null)
            {
                return NotFound();
            }
            OfferModel offer = offerService.GetById(offerId);
            if (offer == null)
            {
                return NotFound();
            }
            return Created("AddOfferOnVoucher", voucherService.PutOfferOnVoucher(id, offerId));        
        } 

        [ResponseType(typeof(VoucherModel))]
        [Route("{id}/put-user-to-voucher/{userId}", Name = "AddUserToVoucher")]
        public IHttpActionResult PutUserToVoucher(int id, int userId)
        {
            VoucherModel voucher = voucherService.GetVoucherById(id);
            if (voucher == null)
            {
                return NotFound();
            }
            UserModel user = usersService.GetById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Created("AddUserToVoucher", voucherService.PutUserToVoucher(id, userId));
        } */

        [Route("findByBuyer/{buyerId}")]
        [ResponseType(typeof(VoucherModel))]
        public IHttpActionResult GetVoucherByBuyers(int buyerId)
        {
            UserModel user = usersService.GetById(buyerId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Vouchers);
        }
        [Route("findByOffer/{offerId}")]
        [ResponseType(typeof(VoucherModel))]
        public IHttpActionResult GetVoucherByOffers(int offerId)
        {
            OfferModel offer = offerService.GetById(offerId);
            if (offer == null)
            {
                return NotFound();
            }
            return Ok(offer.Vouchers);
        }
        [Route("findNonExpiredVoucher")]
        public IEnumerable<VoucherModel> GetNonExpiredVouchers()
        {
            return voucherService.GetNonExpiredVouchers();
        }
        [Route("{id}")]
        [ResponseType(typeof(VoucherModel))]
        public IHttpActionResult DeleteVoucher(int id)
        {
            VoucherModel voucher = voucherService.DeleteVoucher(id);

            if (voucher == null)
            {
                return NotFound();
            }
            return Ok(voucher);
        }
        [Route("email/{id}")]
        [HttpGet]
        public IHttpActionResult SendVoucherByEmail(int id)
        {
            VoucherModel voucher = voucherService.GetVoucherById(id);
            
            string to = "exyukosarka@gmail.com";
            string from = "zlaja90ns@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Using the new SMTP client.";
            message.IsBodyHtml = true;
            message.Body = "<tr>\r\n" +
                           "<th>Buyer</th>\r\n" +
                           "<th>Offer</th>\r\n" +
                           "<th>Price</th>\r\n" +
                           "<th>Expire date</th>\r\n" +
                           "</tr>\r\n" +
                           "<tr>\r\n" +
                           "<td>" + voucher.User.FirstName + " " + voucher.User.LastName + "</td>\r\n" +
                           "<td>" + voucher.Offer.OfferName + "</td>\r\n" +
                           "<td>" + voucher.Offer.ActionPrice.ToString() + "</td>\r\n" +
                           "<td>" + voucher.ExpirationDate.ToString() + "</td>\r\n" +
                           "</tr>";
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new System.Net.NetworkCredential("zlaja90ns@gmail.com", "pijanibaxter");
            client.EnableSsl = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                            ex.ToString());

            }
            return Ok(voucher);
        }
    }
}