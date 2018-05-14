using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IOfferService
    {
        IEnumerable<OfferModel> GetOffers();
        OfferModel GetById(int id);
        IEnumerable<OfferModel> GetPriceRange(int lowPrice, int highPrice);
        OfferModel PostOffer(OfferModel offer);
        OfferModel PutOffer(int id, OfferModel updatedOffer);
        OfferModel PutOffer(OfferModel offer, bool isBillCreated);
        OfferModel PutOfferStatus(int id, OfferStatus newOfferStatus);
       /* OfferModel PutCategoryToOffer(int id, int categoryId = 0);
        OfferModel PutUserThatCreatedOffer(int id, int userId);
        OfferModel PutCategoryInOffer(int id, OfferModel updatedOffer);*/
        OfferModel DeleteOffer(int id);

    }
}
