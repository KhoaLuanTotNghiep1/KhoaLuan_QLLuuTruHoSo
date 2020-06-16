using AutoMapper;
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
using System.Web.Http.Description;

namespace S3Train.WebHeThong.Controllers.API
{
    public class UserAPIController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMuonTraService _muonTraService;

        public UserAPIController()
        {

        }

        public UserAPIController(IUserService userService, IMuonTraService muonTraService)
        {
            _userService = userService;
            _muonTraService = muonTraService;
        }

        public async Task<IHttpActionResult> Get()
        {

            var result = await _userService.GetAllAsync();

            return Ok(result.ToList().Select(Mapper.Map<ApplicationUser, UserViewModel>));
        }

        [ResponseType(typeof(UserViewModel))]
        public async Task<IHttpActionResult> GetLogin(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                return BadRequest();

            var result = await _userService.GetUserByUserNameAndPassword(userName, passWord);

            if (result == null)
                return NotFound();

            var user = Mapper.Map<ApplicationUser, UserViewModel>(result);

            return Ok(user);
        }

        public async Task<IHttpActionResult> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var result = await _userService.GetUserByEmail(email);

            if (result == null)
                return NotFound();

            return Ok();
        }

        public async Task<IHttpActionResult> GetById(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return BadRequest();

            var result = await _userService.GetUserById(Id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        public async Task<IHttpActionResult> GetByUser(string user)
        {
            if (string.IsNullOrEmpty(user))
                return BadRequest();

            var result = await _userService.GetUserByUserName(user);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
