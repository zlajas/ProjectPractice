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
using Project.Services;

namespace Project.Controllers
{
    [RoutePrefix("project/users")]
    public class UserController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IUsersService usersService;
        public UserController (IUsersService usersService)
        {
            this.usersService = usersService;
        }
        [Route("public")]
        public IEnumerable<PublicUserDTO> GetUsers()
        {
            logger.Info("Requesting buyers");
            return usersService.GetUsers().Select(x => new PublicUserDTO(x));
        }

        [Route("private")]
        public IEnumerable<PrivateUserDTO> GetPrivateView()
        {
            return usersService.GetUsers().Select(x => new PrivateUserDTO(x));
        }

        [Route("admin")]
        public IEnumerable<AdminUserDTO> GetAdminView()
        {
            return usersService.GetUsers().Select(x => new AdminUserDTO(x));
        }
        
        [Route("{id}")]
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult GetUser(int id)
        {
            UserModel user = usersService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [ResponseType(typeof(UserModel))]
        [Route("api/user/{username}")]
        public IHttpActionResult GetUserByUsername(string username)
        {
            UserModel user = usersService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [Route("")]
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult PostUser(UserDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (newUser.Password != newUser.RepeatedPassword)
            {
                return BadRequest("Password field has to be the same as repeatedPassword field.");
            }

            UserModel user = new UserModel();
            user.FirstName = newUser.FirstName;
            user.LastName = newUser.LastName;
            user.Username = newUser.Username;
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.UserRole = UserRole.ROLE_CUSTOMER;

            usersService.PostUser(user);

            return Created("", user);
        }
        
        [Route("{id}")]
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult PutUser(int id, UserModel user)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }
            UserModel updatedUser = usersService.PutUser(id, user);
            if(updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [ResponseType(typeof(UserModel))]
        [Route("change/{id}/role/{userRole}")]
        public IHttpActionResult PutUserRole(int id, UserRole userRole)
        {
            UserModel user = (usersService.PutUserRole(id, userRole));
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

        }
        [ResponseType(typeof(UserModel))]
        [Route("api/user/changePassword/{id}")]
        public IHttpActionResult PutNewPassword(int id, [FromUri] string oldPass, [FromUri] string newPass)
        {
            UserModel userWithNewPass = usersService.PutNewPassword(id, oldPass, newPass);
            if (userWithNewPass == null)
            {
                return NotFound();
            }
            return Ok(userWithNewPass);
        }

        [Route("api/user/{id}")]
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult DeleteUser(int id)
        {
            UserModel user = usersService.DeleteUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
    }
}
