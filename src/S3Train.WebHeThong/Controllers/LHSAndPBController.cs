using S3Train.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S3Train.WebHeThong.Controllers
{
    public class LHSAndPBController : Controller
    {
        private readonly ILoaiHoSoService _loaiHoSoService;
        private readonly IPhongBanService _phongBanService;

        public LHSAndPBController()
        {

        }

        public LHSAndPBController(ILoaiHoSoService loaiHoSoService, IPhongBanService phongBanService)
        {
            _loaiHoSoService = loaiHoSoService;
            _phongBanService = phongBanService;
        }

        // GET: LHSAnfPB

        public ActionResult Index()
        {
            return View();
        }
    }
}