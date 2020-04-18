using S3Train.Contract;
using S3Train.Domain;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class TuController : Controller
    {
        private readonly ITuService _tuService;
        private readonly IKeService _keService;

        public TuController()
        {

        }

        public TuController(ITuService tuService, IKeService keService)
        {
            _tuService = tuService;
            _keService = keService;
        }

        // GET: Tu
        public ActionResult Index()
        {
            var model = GetTus(_tuService.GetAll(x => x.OrderBy(m => m.Ten)));

            return View(model);
        }

        private IList<TuViewModel> GetTus(IList<Tu> tus)
        {
            return tus.Select(x => new TuViewModel
            {
              Id = x.Id,
              Ten = x.Ten,
              DonViTinh = x.DonViTinh,
              DienTich = x.DonViTinh,
              NgươiQuanLy = x.NgươiQuanLy,
              TinhTrang = x.TinhTrang,
              SoLuongHienTai = x.SoLuongHienTai,
              SoLuongMax = x.SoLuongMax,
              ViTri = x.ViTri,
              NgayTao = x.NgayTao,
              NgayCapNhat = x.NgayCapNhat,
              TrangThai = x.TrangThai,
              Kes = x.Kes
            }).ToList();
        }
    }
}