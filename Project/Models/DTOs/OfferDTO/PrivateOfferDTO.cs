using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.OfferDTO
{
    public class PrivateOfferDTO : AdminOfferDTO
    {
        public PrivateOfferDTO()
        { }

        public PrivateOfferDTO(OfferModel offer) : base(offer)
        { }
    }
}