using S3Train.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
