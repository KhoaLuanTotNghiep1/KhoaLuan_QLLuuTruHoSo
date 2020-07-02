using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.IdentityManager;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC)]
    [RoutePrefix("Quyen-Truy-Cap")]
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
        //private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;
        private readonly IRoleService _roleService;

        public RoleController()
        {
        }
        public RoleController(/*IFunctionLichSuHoatDongService functionLichSuHoatDongService,*/ IRoleService roleService)
        {
            //_functionLichSuHoatDongService = functionLichSuHoatDongService;
            _roleService = roleService;
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
            roleManager = _roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Admin/Role
        [Route("Danh-Sach")]
        public ActionResult Index()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();

            foreach (var role in RoleManager.Roles)
            {
                int countUser = _roleService.CountUsersBelongRoleByRoleName(role.Name);
                list.Add(new RoleViewModel(role, countUser));
            }
            return View(list);
        }

        //public async Task<PartialViewResult> CreateOrUpdateAsync(string id)
        //{
        //    var model = new RoleViewModel();
        //    if(string.IsNullOrEmpty(id))
        //    {
        //        return PartialView("~/Areas/Admin/Views/Role/_CreateAndEditRolePartial.cshtml",model);
        //    }
        //    else
        //    {
        //        var role = await RoleManager.FindByIdAsync(id);
        //        return PartialView("~/Areas/Admin/Views/Role/_CreateAndEditRolePartial.cshtml", new RoleViewModel(role));
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> CreateOrUpdate(RoleViewModel model)
        //{
        //    if(string.IsNullOrEmpty(model.Id))
        //    {
        //        var role = new ApplicationRole() { Name = model.Name, Description = model.Description };
        //        role.Id = Guid.NewGuid().ToString();

        //        await RoleManager.CreateAsync(role);
        //        _functionLichSuHoatDongService.Create(ActionWithObject.Create, User.Identity.GetUserId(), "quyền truy cập: " + model.Name);
        //        TempData["AlertMessage"] = "Tạo Mới Thành Công";
        //    }
        //    else
        //    {
        //        var role = await RoleManager.FindByIdAsync(model.Id);
        //        role.Name = model.Name;
        //        role.Description = model.Description;

        //        await RoleManager.UpdateAsync(role);
        //        _functionLichSuHoatDongService.Create(ActionWithObject.Update, User.Identity.GetUserId(), "quyền truy cập: " + model.Name);
        //        TempData["AlertMessage"] = "Cập Nhật Thành Công";
        //    }
        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> Delete(string id)
        //{
        //    var role = await RoleManager.FindByIdAsync(id);
        //    await RoleManager.DeleteAsync(role);
        //    TempData["AlertMessage"] = "Xóa Thành Công";
        //    _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "quyền truy cập: " + role.Name);
        //    return RedirectToAction("Index");
        //}
    }
}