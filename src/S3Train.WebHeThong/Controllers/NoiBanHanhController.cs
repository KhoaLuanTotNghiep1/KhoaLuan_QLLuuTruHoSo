using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
    [RoutePrefix("Noi-Ban-Hanh")]
    public class NoiBanHanhController : Controller
    {
        private readonly INoiBanHanhService _noiBanHanhService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public NoiBanHanhController()
        {

        }

        public NoiBanHanhController(INoiBanHanhService noiBanHanhService, IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _noiBanHanhService = noiBanHanhService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: NoiBanHanh
        [Route("Danh-Sach")]
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new NoiBanHanhIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var noiBanHanhs = _noiBanHanhService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.Ten));

            model.Paged = noiBanHanhs;
            model.Items = GetNoiBanHanhs(noiBanHanhs.ToList());
            return View(model);
        }

        [HttpGet]
        public PartialViewResult CreateOrUpdate(string id)
        {
            var model = new NoiBanHanhViewModel();
            if (string.IsNullOrEmpty(id))
            {
                return PartialView("~/Views/Role/_PartialCreateOrUpdateRole.cshtml", model);
            }
            else
            {
                var noiBanHanh = _noiBanHanhService.Get(m => m.Id == id);
                model.Id = noiBanHanh.Id;
                model.Ten = noiBanHanh.Ten;
                model.MoTa = noiBanHanh.MoTa;
                model.TrangThai = noiBanHanh.TrangThai;
                model.NgayTao = noiBanHanh.NgayTao;
                model.NgayCapNhat = noiBanHanh.NgayCapNhat;

                return PartialView("~/Views/Role/_PartialCreateOrUpdateRole.cshtml", model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(NoiBanHanhViewModel model)
        {
            var noiBanHanh = string.IsNullOrEmpty(model.Id) ? new NoiBanHanh { NgayCapNhat = DateTime.Now }
                : _noiBanHanhService.Get(m => m.Id == model.Id);

            noiBanHanh.Ten = model.Ten;
            noiBanHanh.MoTa = model.MoTa;
            noiBanHanh.TrangThai = true;

            if (string.IsNullOrEmpty(model.Id))
            {
                noiBanHanh.Id = Guid.NewGuid().ToString();
                noiBanHanh.NgayTao = DateTime.Now;
                _noiBanHanhService.Insert(noiBanHanh);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, User.Identity.GetUserId(), "nơi ban hành: " + model.Ten);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                noiBanHanh.NgayCapNhat = DateTime.Now;
                _noiBanHanhService.Update(noiBanHanh);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, User.Identity.GetUserId(), "nơi ban hành: " + model.Ten);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var noiBanHanh = _noiBanHanhService.Get(m => m.Id == id);
            _noiBanHanhService.Remove(noiBanHanh);
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), "nơi ban hành: " + noiBanHanh.Ten);
            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index");
        }

        private List<NoiBanHanhViewModel> GetNoiBanHanhs(IList<NoiBanHanh> noiBanHanhs)
        {
            return noiBanHanhs.Select(x => new NoiBanHanhViewModel
            {
                Id = x.Id,
                Ten = x.Ten,
                MoTa = x.MoTa,
                TaiLieuVanBans = x.TaiLieuVanBans,
                NgayTao = x.NgayTao,
                NgayCapNhat = x.NgayCapNhat,
                TrangThai = x.TrangThai,
            }).ToList();
        }
    }
}