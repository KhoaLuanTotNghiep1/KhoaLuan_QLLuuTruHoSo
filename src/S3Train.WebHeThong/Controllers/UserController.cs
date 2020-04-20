using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
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
        public async Task<ActionResult> IndexAsync()
        {
            var model = await _userService.GetUser(1,10);
            ViewBag.Roles = DropDownRole();
            return View(model);
        }

        [HttpGet]
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

                await _userService.Create(user, model.Password);
                await _userService.UserAddToRoles(user.Id, model.Role);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
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
            }
            return RedirectToAction("IndexAsync");
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userService.GetUserById(id);
            await _userService.DeleteAsync(user);
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("IndexAsync");
        }

        public async Task<ActionResult> ChangeRole(UserViewModel model)
        {
            var roles = await _userService.GetRolesForUser(model.Id);

            await _userService.RemoveFromRoles(model.Id, roles[0].ToString());
            await _userService.UserAddToRoles(model.Id, model.Role);

            TempData["AlertMessage"] = "Thay Đổi Quyền Thành Công";
            return RedirectToAction("IndexAsync");
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