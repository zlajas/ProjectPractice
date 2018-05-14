using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.OfferDTO
{
    public class PublicOfferDTO : AdminOfferDTO
    {
        public PublicOfferDTO()
        { }
        public PublicOfferDTO(OfferModel offer) : base(offer)
        { }
    }
}