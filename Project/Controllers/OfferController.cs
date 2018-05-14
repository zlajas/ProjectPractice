using Project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Project.Repositories;
using Project.Services;
using System.Threading.Tasks;
using System.Web;
using Project.Models.DTOs.OfferDTO;
using Project.Models.DTOs;

namespace Project.Controllers
{
    [RoutePrefix("project/offers")]
    public class OfferController : ApiController
    {
        private IOfferService offerService;
        private ICategoryService categoryService;
        private IUsersService usersService;

        public OfferController (IOfferService offerService, ICategoryService categoryService, IUsersService usersService)
        {
            this.offerService = offerService;
            this.categoryService = categoryService;
            this.usersService = usersService;
        }
        [Route("public")]
        public IEnumerable<PublicOfferDTO> GetPublicOffersView()
        {
            return offerService.GetOffers().Select(x => new PublicOfferDTO(x));         
        }

        [Route("private")]
        public IEnumerable<PrivateOfferDTO> GetPrivateOffersView()
        {
            return offerService.GetOffers().Select(x => new PrivateOfferDTO(x));
        }

        [Route("admin")]
        public IEnumerable<AdminOfferDTO> GetAdminOffersView()
        {
            return offerService.GetOffers().Select(x => new AdminOfferDTO(x));
        }

        [Route("{id}")]
        [ResponseType(typeof(OfferModel))]
        public IHttpActionResult GetOffer(int id)
        {
            OfferModel offer = offerService.GetById(id);
            if (offer == null)
            {
                return NotFound();
            }
           
            return Ok(offer);
        }
        [ResponseType(typeof(OfferModel))]
        [Route("findByPrice/{lowPrice}/to/{highPrice}")]
        public IHttpActionResult GetPriceRange(int lowPrice, int highPrice)
        {   
            return Ok(offerService.GetPriceRange(lowPrice, highPrice));
        }
        [Route("", Name ="PostOffer")]
        [ResponseType(typeof(OfferModel))]
        public IHttpActionResult PostOffer(OfferModel offer)
        {
            if (!ModelState.IsValid || offer.CategoryId == null || offer.SellerId == null )
            {
                return BadRequest(ModelState);
            }

            CategoryModel category = categoryService.GetById((int)offer.CategoryId);
            UserModel seller = usersService.GetById((int)offer.SellerId);

            if(category == null || seller == null)
            {
                return NotFound();
            }
            if(seller.UserRole != UserRole.ROLE_SELLER)
            {
                return BadRequest("User's role must be ROLE_SELLER");
            }

            offer.Category = category;
            offer.User = seller;
            OfferModel createdOffer = offerService.PostOffer(offer);

            return CreatedAtRoute("PostOffer", new { id = createdOffer.Id}, createdOffer);
        }

        [Route("{id}")]
        [ResponseType(typeof(OfferModel))]
        public IHttpActionResult PutOffer(int id, OfferModel updatedOffer)
        {
            if (!ModelState.IsValid || updatedOffer.CategoryId == null || updatedOffer.SellerId == null)
            {
                return BadRequest(ModelState);
            }

            if (id != updatedOffer.Id)
            {
                return BadRequest();
            }

            CategoryModel category = categoryService.GetById((int)updatedOffer.CategoryId);
            UserModel seller = usersService.GetById((int)updatedOffer.SellerId);

            if(category == null || seller == null)
            {
                return NotFound();
            }

            if(seller.UserRole != UserRole.ROLE_SELLER)
            {
                return BadRequest("User's rolle must be ROLE_SELLER");
            }

            updatedOffer.Category = category;
            updatedOffer.User = seller;

            OfferModel updatedOffer1 = offerService.PutOffer(id, updatedOffer);

            if (updatedOffer1 == null)
            {
                return NotFound();
            }

            return Ok(updatedOffer1);
        }
        [Route("changeOffer/{id}/status/{status}")]
        [ResponseType(typeof(OfferModel))]
        public IHttpActionResult PutOfferStatus(int id, OfferStatus status)
        {
            OfferModel offerWithNewStatus = offerService.PutOfferStatus(id, status);

            if (offerWithNewStatus == null)
            {
                return NotFound();
            }
            return Ok(offerWithNewStatus);
        }

       /* [Route("update-category/{id}")]
        [ResponseType(typeof(OfferModel))]
        public IHttpActionResult PutCategoryInOffer (int id, OfferModel updatedOffer)
        {
            OfferModel offer = offerService.PutCategoryInOffer(id, updatedOffer);
            if (offer == null)
            {
                return NotFound();
            }
            return Ok(offer);
        }*/

        [Route("{id}")]
        [ResponseType(typeof(OfferModel))]
        public IHttpActionResult DeleteOffer(int id)
        {
            OfferModel offer = offerService.DeleteOffer(id);

            if (offer == null)
            {
                return NotFound();
            }
            return Ok(offer);
        }

        [Route("uploadImage/{id}")]
        public async Task<HttpResponseMessage> PostOfferImage(int id)
        {
            OfferModel offer = offerService.GetById(id);
            if (offer == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            offer.ImagePath = HttpContext.Current.Server.MapPath("~/Models/offerImage");
            var provider = new MultipartFormDataStreamProvider(offer.ImagePath);
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);
                }
                return Request.CreateResponse(offer);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            
        }       
    }
    
}
