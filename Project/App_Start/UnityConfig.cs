using System.Data.Entity;
using System.Web.Http;
using Project.Models;
using Project.Models.DTOs;
using Project.Repositories;
using Project.Services;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace Project
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IGenericRepository<UserModel>, GenericRepository<UserModel>>();
            container.RegisterType<IGenericRepository<CategoryModel>, GenericRepository<CategoryModel>>();
            container.RegisterType<IGenericRepository<OfferModel>, GenericRepository<OfferModel>>();
            container.RegisterType<IGenericRepository<BillModel>, GenericRepository<BillModel>>();
            container.RegisterType<IGenericRepository<VoucherModel>, GenericRepository<VoucherModel>>();
            container.RegisterType<DbContext, DataAccessContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IOfferService, OfferService>();
            container.RegisterType<IBillService, BillService>();
            container.RegisterType<IVoucherService, VoucherService>();


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}