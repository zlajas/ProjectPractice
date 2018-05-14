using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models.DTOs.OfferDTO
{
    public class AdminOfferDTO
    {
        public int Id { get; set; }
        public string OfferName { get; set; }
        public string OfferDescription { get; set; }
        public DateTime OfferCreated { get; set; }
        public DateTime OfferExpires { get; set; }
        public double RegularPrice { get; set; }
        public double ActionPrice { get; set; }
        public string ImagePath { get; set; }
        public int AvailableOffers { get; set; }
        public int BoughtOffers { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public OfferStatus OfferStatus { get; set; }
        public virtual UserModel Seller { get; set; }
        public virtual CategoryModel Category { get; set; }

        public AdminOfferDTO()
        { }

        public AdminOfferDTO(OfferModel offer)
        {
            Id = offer.Id;
            OfferName = offer.OfferName;
            OfferDescription = offer.OfferDescription;
            OfferCreated = offer.OfferCreated;
            OfferExpires = offer.OfferExpires;
            RegularPrice = offer.RegularPrice;
            ActionPrice = offer.ActionPrice;
            ImagePath = offer.ImagePath;
            AvailableOffers = offer.AvailableOffers;
            BoughtOffers = offer.BoughtOffers;
            OfferStatus = offer.OfferStatus;
            Seller = offer.User;
            Category = offer.Category;
        }

    }
}