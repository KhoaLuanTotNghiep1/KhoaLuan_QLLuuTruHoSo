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
    public class MuonTraController : Controller
    {
        private readonly IMuonTraService _muonTraService;
        //private readonly IKeService _keService;

        public MuonTraController()
        {

        }

        public MuonTraController(IMuonTraService muonTraService)
        {
            _muonTraService = muonTraService;
        }
        // GET: MuonTra
        public ActionResult Index(int? pageIndex, int? pageSize)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            var model = new MuonTraIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var muontras = _muonTraService.GetAllPaged(pageIndex, pageSize.Value, null, p => p.OrderBy(c => c.NgayMuon));

            model.Paged = muontras;
            model.Items = GetMuonTras(muontras.ToList());
            return View(model);
        }

        private List<MuonTraViewModel> GetMuonTras(IList<MuonTra> muontras)
        {
            return muontras.Select(x => new MuonTraViewModel
            {
                Id = x.Id,
                Ten = x.VanThu,
                NgayMuon = x.NgayMuon,
                NgayTra = x.NgayKetThuc,
                NgayCapNhat = x.NgayCapNhat,
                TinhTrang = x.TinhTrang,
                TrangThai = x.TrangThai,
                UserId = x.UserId,
            }).ToList();
        }
    }
}