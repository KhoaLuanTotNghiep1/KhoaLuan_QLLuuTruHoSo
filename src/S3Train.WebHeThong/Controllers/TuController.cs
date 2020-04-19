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
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new TuIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var tus = _tuService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.Ten));

            model.Paged = tus;
            model.Items = GetTus(tus.ToList());
            return View(model);
        }

        private List<TuViewModel> GetTus(IList<Tu> tus)
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