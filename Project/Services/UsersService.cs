using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using NLog;
using Project.Models;
using Project.Models.DTOs;
using Project.Repositories;

namespace Project.Services
{
    public class UsersService: IUsersService
    {
        private IUnitOfWork db;
        public UsersService(IUnitOfWork db)
        {
            this.db = db;
        }
        public IEnumerable<UserModel> GetUsers()
        {
            return db.UserModelRepository.Get();
        }
        public UserModel GetById(int id)
        {
            return db.UserModelRepository.GetByID(id);
            
        }
        
        public UserModel GetUserByUsername(string username)
        {
            return db.UserModelRepository.Get().FirstOrDefault(x => x.Username == username);
            
        }

        
        public UserModel PostUser(UserModel user)
        {         
            db.UserModelRepository.Insert(user);
            db.Save();
            return user;
        }

        public UserModel PutUser(int id, UserModel updatedUser)
        {
            UserModel user = db.UserModelRepository.GetByID(id);

            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Username = updatedUser.Username;
                user.Email = updatedUser.Email;

                db.UserModelRepository.Update(user);
                db.Save();
            }

            return user;
        }

        public UserModel PutUserRole(int id, UserRole userRole)
        {
            UserModel user = db.UserModelRepository.GetByID(id);
            if (user != null)
            {
                user.UserRole = userRole;
                db.UserModelRepository.Update(user);
                db.Save();
            }
            return user;

        }
        public UserModel PutNewPassword(int id, [FromUri] string oldPass, [FromUri] string newPass)
        {
            UserModel user = db.UserModelRepository.GetByID(id);
            if (user == null)
            {
                return null;
            }
            if (user.Password == oldPass)
            {
                user.Password = newPass;
                db.UserModelRepository.Update(user);
                db.Save();
                return user;
            }
            else
            {
                return null;
            }
        }
        [Route("api/user/{id}")]
        [ResponseType(typeof(UserModel))]
        public UserModel DeleteUser(int id)
        {
            UserModel user = db.UserModelRepository.GetByID(id);
            if (user == null)
            {
                return null;
            }
            db.UserModelRepository.Delete(user);
            db.Save();

            return user;
        }
    }
}