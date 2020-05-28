using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace S3Train.WebHeThong.Controllers.API
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController()
        {

        }

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IHttpActionResult> GetLogin(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                return BadRequest();
            var user = await _userService.GetUserByUserNameAndPassword(userName, passWord);

            if (user != null)
                return Ok(Mapper.Map<ApplicationUser,UserViewModel>(user));
            return NotFound();
        }
    }
}
