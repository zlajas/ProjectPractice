using Project.Repositories;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Services
{
    public class OfferService: IOfferService
    {
        private IUnitOfWork db;
        public OfferService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<OfferModel> GetOffers()
        {
            return db.OfferModelRepository.Get();
        }
        public OfferModel GetById(int id)
        {
            return db.OfferModelRepository.GetByID(id);
        }
        public IEnumerable<OfferModel> GetPriceRange(int lowPrice, int highPrice)
        {
            return db.OfferModelRepository.Get(x => x.ActionPrice >= lowPrice && x.ActionPrice <= highPrice);
        }


        public OfferModel PostOffer(OfferModel offer)
        {
            offer.OfferStatus = OfferStatus.WAIT_FOR_APPROVING;
            offer.OfferCreated = DateTime.UtcNow;
            offer.OfferExpires = offer.OfferCreated.AddDays(10);

            db.OfferModelRepository.Insert(offer);
            db.Save();

            return offer;
        }

        public OfferModel PutOffer(int id, OfferModel updatedOffer)
        {
            OfferModel offer = db.OfferModelRepository.GetByID(id);
            if (offer != null)
            {
                offer.Id = updatedOffer.Id;
                offer.OfferName = updatedOffer.OfferName;
                offer.OfferDescription = updatedOffer.OfferDescription;
                offer.OfferCreated = updatedOffer.OfferCreated;
                offer.ImagePath = updatedOffer.ImagePath;
                offer.OfferExpires = updatedOffer.OfferExpires;
                offer.RegularPrice = updatedOffer.RegularPrice;
                offer.ActionPrice = updatedOffer.ActionPrice;
                offer.AvailableOffers = updatedOffer.AvailableOffers;
                offer.BoughtOffers = updatedOffer.BoughtOffers;

                db.OfferModelRepository.Update(offer);
                db.Save();
            }
            return offer;
        }

        public OfferModel PutOffer(OfferModel offer, bool isBillCreated)
        {
            if (isBillCreated)
            {
                offer.AvailableOffers--;
                offer.BoughtOffers++;
            }
            else
            {
                offer.AvailableOffers++;
                offer.BoughtOffers--;
            }

            db.OfferModelRepository.Update(offer);
            db.Save();

            return offer;
        }

        public OfferModel PutOfferStatus(int id, OfferStatus newOfferStatus)
        {
            OfferModel offer = db.OfferModelRepository.GetByID(id);

            if (offer == null)
            {
                return null;
            }
            offer.OfferStatus = newOfferStatus;
            if ((int)offer.OfferStatus == 4)
            {
                foreach (BillModel bill in offer.Bills)
                {
                    bill.PaymentCanceled = true;
                    db.OfferModelRepository.Update(offer);
                    db.Save();
                    return offer;
                }
            }
            return null;
        }
      /*  public OfferModel PutCategoryToOffer(int id, int categoryId = 0)
        {

            OfferModel offer = db.OfferModelRepository.GetByID(id);
            if (offer == null)
            {
                return null;
            }
            CategoryModel category = db.CategoryModelRepository.GetByID(categoryId);
            if (category == null)
            {
                return null;
            }
            offer.Category = category;
            db.OfferModelRepository.Update(offer);

            db.Save();
            return offer;
        }
        public OfferModel PutUserThatCreatedOffer(int id, int userId)
        {
            OfferModel offer = db.OfferModelRepository.GetByID(id);
            if (offer == null)
            {
                return null;
            }
            UserModel user = db.UserModelRepository.GetByID(userId);
            if (user == null)
            {
                return null;
            }
            if ((int)user.UserRole == 3)
            {
                offer.User = user;
                db.OfferModelRepository.Update(offer);
                db.Save();
                return offer;
            }
            else
            {
                return null;
            }

        }
        public OfferModel PutCategoryInOffer(int id, OfferModel updatedOffer)
        {
            OfferModel offer = db.OfferModelRepository.GetByID(id);
            if (offer == null)
            {
                return null;
            }

            offer.Category.CategoryName = updatedOffer.Category.CategoryName;
            offer.Category.CategoryDescription = updatedOffer.Category.CategoryDescription;
            db.OfferModelRepository.Update(offer);

            db.Save();
            return offer;
        }*/
        public OfferModel DeleteOffer(int id)
        {
            OfferModel offer = db.OfferModelRepository.GetByID(id);
            if (offer == null)
            {
                return null;
            }
            db.OfferModelRepository.Delete(id);
            db.Save();

            return offer;
        }
    }
}