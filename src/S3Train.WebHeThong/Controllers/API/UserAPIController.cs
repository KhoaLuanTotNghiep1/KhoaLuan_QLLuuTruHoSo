using AutoMapper;
using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model.Dto;
using S3Train.Model.User;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.IO;
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

        public UserAPIController()
        {

        }

        public UserAPIController(IUserService userService, IAccountManager accountManager)
        {
            _userService = userService;
        }

        public async Task<IHttpActionResult> Get()
        {
            var result = await _userService.GetAllAsync();

            return Ok(result.ToList().Select(Mapper.Map<ApplicationUser, UserDto>));
        }

        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetLogin(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                return BadRequest();

            var result = await _userService.GetUserByUserNameAndPassword(userName, passWord);

            if (result == null)
                return NotFound();

            return Ok(Mapper.Map<ApplicationUser, UserDto>(result));
        }

        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetById(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return BadRequest();

            var result = await _userService.GetUserById(Id);

            if (result == null)
                return NotFound();

            return Ok(Mapper.Map<ApplicationUser, UserDto>(result));
        }

        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest();

            var result = await _userService.GetUserByUserName(userName);

            if (result == null)
                return NotFound();

            return Ok(Mapper.Map<ApplicationUser, UserDto>(result));
        }

        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var result = await _userService.GetUserByEmail(email);

            if (result == null)
                return NotFound();

            return Ok(Mapper.Map<ApplicationUser, UserDto>(result));
        }

        [HttpPut]
        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> PutUser(string id, UserDto userDto)
        {
            if (string.IsNullOrEmpty(id) || !ModelState.IsValid)
                return BadRequest();

            var user = await _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            var userUpdate = Mapper.Map(userDto, user);

            userUpdate.UpdatedDate = DateTime.Now;
            await _userService.Update(userUpdate);

            return Ok(userDto);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutUpdatePassWord(UpdatePassWord model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userService.GetUserById(model.Id);

            if (user == null)
                return NotFound();

            var result = await _userService.UpdatePassword(user.Id, model.PassWord);
            
            if(result.Errors.Count() > 0)
            {
                string error = "";

                foreach (var er in result.Errors)
                    error += er.ToString();

                return BadRequest(error);
            }

            return Ok(Mapper.Map<ApplicationUser, UserDto>(user));
        }
    }
}
