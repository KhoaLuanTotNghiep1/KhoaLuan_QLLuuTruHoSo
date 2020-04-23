﻿using S3Train.Contract;
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
    public class NoiBanHanhController : Controller
    {
        private readonly INoiBanHanhService _noiBanHanhService;

        public NoiBanHanhController()
        {

        }

        public NoiBanHanhController(INoiBanHanhService noiBanHanhService)
        {
            _noiBanHanhService = noiBanHanhService;
        }

        // GET: NoiBanHanh
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
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                noiBanHanh.NgayCapNhat = DateTime.Now;
                _noiBanHanhService.Update(noiBanHanh);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var tu = _noiBanHanhService.Get(m => m.Id == id);
            _noiBanHanhService.Remove(tu);
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