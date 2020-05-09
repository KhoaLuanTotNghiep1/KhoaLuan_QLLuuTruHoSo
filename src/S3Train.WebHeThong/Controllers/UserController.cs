using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.Model.User;
using S3Train.WebHeThong.CommomClientSide.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        // nhớ làm thêm chức năng kiểm tra email và username đã tồn tại khi lập tài khoản user mới
        public UserController()
        {

        }
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // GET: User
        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public async Task<ActionResult> IndexAsync()
        {
            var model = await _userService.GetUser(1,10);
            ViewBag.Roles = DropDownRole();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public async Task<ActionResult> CreateOrUpdate(string id)
        {
            ViewBag.Roles = DropDownRole();
            if (string.IsNullOrEmpty(id))
            {
                return View(new UserViewModel());
            }
            else
            {
                var user = await _userService.GetUserById(id);
                return View(new UserViewModel(user));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrUpdate(UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                var user = new ApplicationUser()
                {
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    Address = model.Address,
                    FullName = model.FullName,
                    CreatedDate = DateTime.Now,
                    Avatar = "boy.png",
                    Active = true
                };

                var result = await _userService.Create(user, model.Password);

                if (result.Succeeded)
                {
                    await _userService.UserAddToRoles(user.Id, model.Role);
                    TempData["AlertMessage"] = "Tạo Mới Thành Công";
                    return RedirectToAction("IndexAsync");
                }
                else
                {
                    ViewBag.Roles = DropDownRole();
                    foreach (var er in result.Errors)
                        TempData["AlertMessage"] += er.ToString();
                    return View(model);
                }
            }
            else
            {
                var user = await _userService.GetUserById(model.Id);
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.UserName;
                user.Address = model.Address;
                user.FullName = model.FullName;
                user.UpdatedDate = DateTime.Now;

                await _userService.Update(user);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
                return RedirectToAction("UserProfile");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeAvatar(HttpPostedFileBase file)
        {
            string local = Server.MapPath("~/Content/Avatar/");

            if (file != null)
            {
                string id = User.Identity.GetUserId();

                var user = await _userService.GetUserById(id);

                user.Avatar = UploadFile.UpFileAndGetFileName(file, local);

                await _userService.Update(user);

                TempData["AlertMessage"] = "Cập nhật avatar thành công";
            }
            else
            {
                TempData["AlertMessage"] = "Cập nhật avatar thất bại";
            }

            return RedirectToAction("UserProfile");
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userService.GetUserById(id);
            await _userService.DeleteAsync(user);
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("IndexAsync");
        }

        public async Task<ActionResult> UserProfile()
        {
            string id = User.Identity.GetUserId();

            var user = await _userService.GetUserById(id);
            var roles = await _userService.GetRolesForUser(id);

            var model = new UserViewModel(user);

            model.Role = roles.Count() > 0 ? roles[0].ToString() : "";
            return View(model);
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public async Task<ActionResult> ChangeRole(UserViewModel model)
        {
            var roles = await _userService.GetRolesForUser(model.Id);
            if(roles.Count > 0)
                await _userService.RemoveFromRoles(model.Id, roles[0].ToString());
            await _userService.UserAddToRoles(model.Id, model.Role);

            TempData["AlertMessage"] = "Đổi quyền người dùng thành quyền " + model.Role + " thành công";
            return RedirectToAction("IndexAsync");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassWord(string Password)
        {
            string id = User.Identity.GetUserId();

            var result = await _userService.UpdatePassword(id, Password);

            TempData["AlertMessage"] = "Đổi mật khẩu thành công";

            return RedirectToAction("UserProfile");
        }

        private List<SelectListItem> DropDownRole()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var roles = _roleService.GetAllRoles();
            foreach (var role in roles)
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            return list;
        }
    }
}