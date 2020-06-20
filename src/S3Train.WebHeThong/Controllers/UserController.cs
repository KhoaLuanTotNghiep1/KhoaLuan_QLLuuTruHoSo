using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.Model.User;
using S3Train.WebHeThong.CommomClientSide.Function;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize]
    [RoutePrefix("Nguoi-Dung")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAccountManager _accountManager;
        private readonly IRoleService _roleService;
        private readonly IMuonTraService _muonTraService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public UserController()
        {

        }
        public UserController(IUserService userService, IRoleService roleService, IMuonTraService muonTraService,
            IFunctionLichSuHoatDongService functionLichSuHoatDongService, IAccountManager accountManager)
        {
            _userService = userService;
            _roleService = roleService;
            _muonTraService = muonTraService;
            _accountManager = accountManager;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: User
        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        [Route("Danh-Sach")]
        public async Task<ActionResult> IndexAsync()
        {
            var model = await _userService.GetUser(1,GlobalConfigs.DEFAULT_PAGESIZE);
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
                    _functionLichSuHoatDongService.Create(ActionWithObject.Create, User.Identity.GetUserId(), "tài khoản: " + user.UserName);
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
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "tài khoản: " + user.UserName);
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("IndexAsync");
        }

        [Route("Thong-Tin-Nguoi-Dung")]
        public async Task<ActionResult> UserProfile()
        {
            string id = User.Identity.GetUserId();

            var user = await _userService.GetUserById(id);
            var roles = await _userService.GetRolesForUser(id);

            var model = new UserViewModel(user)
            {
                Role = roles.Count() > 0 ? roles[0].ToString() : ""
            };

            ViewBag.MuonTras = _muonTraService.Gets(p => p.UserId == id && p.TinhTrang == EnumTinhTrang.DangMuon);

            return View(model);
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public async Task<ActionResult> ChangeRole(UserViewModel model)
        {
            var roles = await _userService.GetRolesForUser(model.Id);
            var user = await _userService.GetUserById(model.Id);

            if(roles.Count > 0)
                await _userService.RemoveFromRoles(model.Id, roles[0].ToString());

            await _userService.UserAddToRoles(model.Id, model.Role);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(),
                "quyền tài khoản: " + user.UserName + " thành quyền " + model.Role);

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


        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
                return RedirectToAction("NotFound", "Home");
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userService.GetUserByEmail(model.Email);
            if (user == null)
            {
                TempData["AlertMessage"] = "Email Bạn Nhập Không Đúng.";
                return RedirectToAction("ResetPassword", "User");
            }
            var result = await _userService.UpdatePassword(user.Id, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmail(model.Email);

                if (user == null)
                {
                    ViewBag.Error = "Email Bạn Nhập Không Tồn Tại. Làm Ơn Kiểm Tra Lại";
                    return View(model);
                }

                string code = await _accountManager.UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await _accountManager.UserManager.SendEmailAsync(user.Id, "Mật Khẩu", ReadHTMLSendEmail(callbackUrl));
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        private string ReadHTMLSendEmail(string url)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/User/BodySendEmail.cshtml")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Url}", url);

            return body;
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