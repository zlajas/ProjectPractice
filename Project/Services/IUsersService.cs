using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Project.Models;
using Project.Models.DTOs;
using Project.Repositories;

namespace Project.Services
{
    public interface IUsersService
    {
        IEnumerable<UserModel> GetUsers();
        UserModel GetById(int Id);
        UserModel GetUserByUsername(string username);
        UserModel PostUser(UserModel user);
        UserModel PutUser(int id, UserModel user);
        UserModel PutUserRole(int id, UserRole userRole);
        UserModel PutNewPassword(int id, [FromUri] string oldPass, [FromUri] string newPass);
        UserModel DeleteUser(int id);



    }
}
