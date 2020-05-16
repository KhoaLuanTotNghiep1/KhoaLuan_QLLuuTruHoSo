using Microsoft.AspNet.Identity;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.DropDownList;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AlgorithmLibrary.Kmeans;
using X.PagedList;
using S3Train.WebHeThong.CommomClientSide.Function;
using System.Web;
using System.IO;
using System.Threading.Tasks;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize]
    public class TaiLieuVanBanController : Controller
    {
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly INoiBanHanhService _noiBanHanhService;
        private readonly IHoSoService _hoSoService;
        private readonly ILoaiHoSoService _loaiHoSoService;
        private readonly IUserService _userService;
        private readonly IFunctionLichSuHoatDongService _functionLichSuHoatDongService;

        public TaiLieuVanBanController()
        {

        }

        public TaiLieuVanBanController(ITaiLieuVanBanService taiLieuVanBanService, INoiBanHanhService noiBanHanhService, 
            IHoSoService hoSoService, ILoaiHoSoService loaiHoSoService, IUserService userService, 
            IFunctionLichSuHoatDongService functionLichSuHoatDongService)
        {
            _taiLieuVanBanService = taiLieuVanBanService;
            _noiBanHanhService = noiBanHanhService;
            _hoSoService = hoSoService;
            _loaiHoSoService = loaiHoSoService;
            _userService = userService;
            _functionLichSuHoatDongService = functionLichSuHoatDongService;
        }

        // GET: TaiLieuVanBan
        public ActionResult Index(int? pageIndex, int? pageSize, string searchString, 
            string dang = GlobalConfigs.DANG_DEN, bool active = true)
        {
            pageIndex = (pageIndex ?? 1);
            pageSize = pageSize ?? GlobalConfigs.DEFAULT_PAGESIZE;

            string[] includeArray = { "NoiBanHanh", "User", "HoSo" };
            var includes = AddList.AddItemByArray(includeArray);

            var model = new TaiLieuVanBanIndexViewModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };

            var taiLieuVanBans = _taiLieuVanBanService.GetAllPaged(pageIndex, pageSize.Value, p => p.TrangThai == active && 
                p.Dang == dang, p => p.OrderBy(c => c.Ten), includes);

            if (!string.IsNullOrEmpty(searchString))
            {
                taiLieuVanBans = _taiLieuVanBanService.GetAllPaged(pageIndex, pageSize.Value, p => p.Ten.Contains(searchString) || p.Loai.Contains(searchString)
                    || p.NoiDung.Contains(searchString) && p.TrangThai == active && p.Dang == dang, p => p.OrderBy(c => c.Ten), includes);
            }

            model.Paged = taiLieuVanBans;
            model.Items = GetTaiLieuVanBans(taiLieuVanBans.ToList());

            ViewBag.Active = active;
            ViewBag.searchString = searchString;
            ViewBag.Controller = "TaiLieuVanBan";
            ViewBag.Dang = dang;

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateOrUpdate(string id)
        {
            var model = new TaiLieu_VanBanViewModel();
            var dangs = new List<object> { GlobalConfigs.DANG_DEN, GlobalConfigs.DANG_NOIBO };

            ViewBag.LoaiHoSos = SelectListItemFromDomain.SelectListItem_LoaiHoSo(_loaiHoSoService.GetAll(m => m.OrderBy(t => t.Ten)));
            ViewBag.NoiBanHanhs = SelectListItemFromDomain.SelectListItem_NoiBanHanh(_noiBanHanhService.GetAll(m => m.OrderBy(t => t.Ten)));
            ViewBag.Dangs = SelectListItemFromDomain.SelectListItem_Object(dangs);

            if (string.IsNullOrEmpty(id))
            {
                return View(model);
            }
            else
            {
                var taiLieuVanBan = _taiLieuVanBanService.Get(m => m.Id == id);
                model = GetTaiLieuVanBan(taiLieuVanBan);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(TaiLieu_VanBanViewModel model, IEnumerable<HttpPostedFileBase> file)
        {
            var taiLieuVanBan = string.IsNullOrEmpty(model.Id) ? new TaiLieuVanBan { NgayCapNhat = DateTime.Now }
                : _taiLieuVanBanService.Get(m => m.Id == model.Id);

            var autoList = AutoCompleteTextHoSos(_hoSoService.GetAll());
            string userId = User.Identity.GetUserId();
            string cthd = model.Loai + ": " + model.Ten;

            string localFile = Server.MapPath("~/Content/HoSo/");
            string localImage = Server.MapPath("~/Content/HinhAnhTLVB/");

            string path = UpFileGetPathOrFileName(file.ElementAt(0), localFile, model.DuongDan,"path");
            string hinhAnh = UpFileGetPathOrFileName(file.ElementAt(1), localImage, model.HinhAnh);

            #region taiLieuVanBan
            taiLieuVanBan.Dang = model.Dang;
            taiLieuVanBan.DuongDan = path;
            taiLieuVanBan.GhiChu = model.GhiChu;
            taiLieuVanBan.HoSoId = autoList.FirstOrDefault(p => p.Text == model.HoSoId).Id;
            taiLieuVanBan.Loai = model.Loai;
            taiLieuVanBan.NguoiDuyet = model.NguoiDuyet;
            taiLieuVanBan.NoiDung = model.NoiDung;
            taiLieuVanBan.NguoiGuiHoacNhan = model.NguoiGuiHoacNhan;
            taiLieuVanBan.NguoiKy = model.NguoiKy;
            taiLieuVanBan.NoiBanHanhId = model.NoiBanHanhId;
            taiLieuVanBan.NoiNhan = model.NoiNhan;
            taiLieuVanBan.SoKyHieu = model.SoKyHieu;
            taiLieuVanBan.SoTo = model.SoTo;
            taiLieuVanBan.Ten = model.Ten;
            taiLieuVanBan.TrichYeu = model.TrichYeu;
            taiLieuVanBan.NgayBanHanh = model.NgayBanHanh;
            taiLieuVanBan.TinhTrang = "Trong Kho";
            taiLieuVanBan.TrangThai = true;
            taiLieuVanBan.UserId = userId;
            taiLieuVanBan.HinhAnh = hinhAnh;
            #endregion

            if (string.IsNullOrEmpty(model.Id))
            {
                taiLieuVanBan.Id = Guid.NewGuid().ToString();
                taiLieuVanBan.NgayTao = DateTime.Now;
                _taiLieuVanBanService.Insert(taiLieuVanBan);
                _functionLichSuHoatDongService.Create(ActionWithObject.Create, userId, cthd);
                TempData["AlertMessage"] = "Tạo Mới Thành Công";
            }
            else
            {
                taiLieuVanBan.NgayCapNhat = DateTime.Now;
                _taiLieuVanBanService.Update(taiLieuVanBan);
                _functionLichSuHoatDongService.Create(ActionWithObject.Update, userId, cthd);
                TempData["AlertMessage"] = "Cập Nhật Thành Công";
            }
            return RedirectToAction("Index", new { dang = model.Dang});
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        [HttpPost]
        public async Task<ActionResult> ChangeDangForTLVB(TaiLieu_VanBanViewModel model)
        {
            var taiLieuVanBan = _taiLieuVanBanService.Get(p => p.Id == model.Id);
            var user = await _userService.GetUserById(User.Identity.GetUserId());

            taiLieuVanBan.NguoiGuiHoacNhan = user.FullName;
            taiLieuVanBan.Dang = GlobalConfigs.DANG_DI;
            taiLieuVanBan.NoiNhan = model.NoiNhan;
            taiLieuVanBan.NgayCapNhat = DateTime.Now;
            taiLieuVanBan.TinhTrang = GlobalConfigs.TINHTRANG_DAGOI;

            _taiLieuVanBanService.Update(taiLieuVanBan);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(),"gởi văn bản đi: " + model.Ten);

            TempData["AlertMessage"] = "Gởi văn bản thành công";
            return RedirectToAction("Index", new { dang = GlobalConfigs.DANG_DI });
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public ActionResult Delete(string id)
        {
            var taiLieuVanBan = _taiLieuVanBanService.Get(m => m.Id == id);
            _taiLieuVanBanService.Remove(taiLieuVanBan);
            TempData["AlertMessage"] = "Xóa Thành Công";
            _functionLichSuHoatDongService.Create(ActionWithObject.Delete, User.Identity.GetUserId(), taiLieuVanBan.Loai + ": " + taiLieuVanBan.Ten);
            return RedirectToAction("Index", new { dang = taiLieuVanBan.Dang});
        }

        public ActionResult Detail(string id)
        {
            var model = GetTaiLieuVanBan(_taiLieuVanBanService.Get(m => m.Id == id));

            return View(model);
        }

        [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
        public ActionResult ChangeActive(string id, bool active)
        {
            var model = _taiLieuVanBanService.Get(m => m.Id == id);

            model.TrangThai = active;
            model.NgayCapNhat = DateTime.Now;

            _taiLieuVanBanService.Update(model);
            _functionLichSuHoatDongService.Create(ActionWithObject.ChangeStatus, User.Identity.GetUserId(),
                model.Loai + ": " + model.Ten + " thành "+ active);

            TempData["AlertMessage"] = "Xóa Thành Công";
            return RedirectToAction("Index", new { dang = model.Dang});
        }

        [HttpPost]
        public ActionResult AutoCompleteText(string text)
        {
            var model = AutoCompleteTextHoSos(_hoSoService.GetAll());

            model = model.Where(p => p.Text.Contains(text)).ToHashSet();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult StorageSuggestion(string document)
        {
            string local = "Không tìm thấy tài liêu/văn bản có cùng nội dung! Tạo hồ sơ mới.";
            var list = _taiLieuVanBanService.GetDocuments();
            list.Add(document);
            var docCollection = new DocumentCollection()
            {
                DocumentList = list
            };

            List<DocumentVector> vSpace = VectorSpaceModel.ProcessDocumentCollection(docCollection);
            List<Centroid> resultSet = DocumnetClustering.DocumentCluster(3, vSpace, document);

            string documentNeedSearch = DocumnetClustering.FindClosestDocument();

            if(!string.IsNullOrEmpty(documentNeedSearch))
            {
                var taiLieuVanBan = _taiLieuVanBanService.Get(p => p.NoiDung == documentNeedSearch);

                local = taiLieuVanBan.HoSo.Hop.Ke.Tu.Ten + " kệ thứ " + taiLieuVanBan.HoSo.Hop.Ke.SoThuTu + 
                    " hộp số " + taiLieuVanBan.HoSo.Hop.SoHop + " hồ sơ " + taiLieuVanBan.HoSo.PhongLuuTru;
            }

            return Json(new { da = local}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DemoKmeans()
        {
            var list = _taiLieuVanBanService.GetDocuments();
            ViewBag.da = "Những bài văn viết về người thầy cũ đã nghỉ hưu, người bố làm nghề xe ôm hay người mẹ đơn thân thần tảo nuôi con… đã lấy được nước mắt của người đọc.";
            list.Add(ViewBag.da);
            var docCollection = new DocumentCollection()
            {
                DocumentList = list
            };
            
            List<DocumentVector> vSpace = VectorSpaceModel.ProcessDocumentCollection(docCollection);
            List<Centroid> resultSet = DocumnetClustering.DocumentCluster(3, vSpace, ViewBag.da);

            ViewBag.data = DocumnetClustering.FindClosestDocument();

            return View(resultSet);
        }

        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="a"></param>
        /// <param name="url">path folder</param>
        /// <param name="get">path/fileName</param>
        /// <returns>path/file name</returns>
        public string UpFileGetPathOrFileName(HttpPostedFileBase a, string url, string name, string get = "fileName")
        {
            string fileName = name;
            if (a != null && a.ContentLength > 0)
            {
                fileName = Path.GetFileName(a.FileName).ToString();
                string path = Path.Combine(url, fileName);
                a.SaveAs(path);

                if (get == "path")
                    return path;
                else
                    return fileName;
            }
            else
            {
                return fileName;
            }
        }

        private HashSet<AutoCompleteTextModel> AutoCompleteTextHoSos(IList<HoSo> hoSos)
        {
            var list = ConvertDomainToAutoCompleteModel.LocalTaiLieu(hoSos);

            return list;
        }

        private TaiLieu_VanBanViewModel GetTaiLieuVanBan(TaiLieuVanBan x)
        {
            var autoList = AutoCompleteTextHoSos(_hoSoService.GetAll());

            var model = new TaiLieu_VanBanViewModel
            {
               Id = x.Id,
               Dang = x.Dang,
               DuongDan = x.DuongDan,
               GhiChu = x.GhiChu,
               HoSo = x.HoSo,
               Loai = x.Loai,
               NgayCapNhat =  x.NgayCapNhat,
               NgayTao = x.NgayTao,
               NguoiDuyet = x.NguoiDuyet,
               NoiDung = x.NoiDung,
               NguoiGuiHoacNhan = x.NguoiGuiHoacNhan,
               NguoiKy = x.NguoiKy,
               NoiBanHanh = x.NoiBanHanh,
               NoiBanHanhId = x.NoiBanHanhId,
               NoiNhan = x.NoiNhan,
               SoKyHieu = x.SoKyHieu,
               SoTo = x.SoTo,
               Ten = x.Ten,
               TinhTrang = x.TinhTrang,
               TrangThai = x.TrangThai,
               TrichYeu = x.TrichYeu,
               User = x.User,
               UserId = x.UserId,
               NgayBanHanh = x.NgayBanHanh,
               HinhAnh = x.HinhAnh,
               HoSoId = x.HoSoId
            };

            model.HoSoId = autoList.FirstOrDefault(p => p.Id == x.HoSoId).Text;
            return model;
        }

        private List<TaiLieu_VanBanViewModel> GetTaiLieuVanBans(IList<TaiLieuVanBan> taiLieuVanBans)
        {
            return taiLieuVanBans.Select(x => new TaiLieu_VanBanViewModel
            {
                Id = x.Id,
                Dang = x.Dang,
                DuongDan = x.DuongDan,
                GhiChu = x.GhiChu,
                HoSo = x.HoSo,
                HoSoId = x.HoSoId,
                Loai = x.Loai,
                NgayCapNhat = x.NgayCapNhat,
                NgayTao = x.NgayTao,
                NguoiDuyet = x.NguoiDuyet,
                NoiDung = x.NoiDung,
                NguoiGuiHoacNhan = x.NguoiGuiHoacNhan,
                NguoiKy = x.NguoiKy,
                NoiBanHanh = x.NoiBanHanh,
                NoiBanHanhId = x.NoiBanHanhId,
                NoiNhan = x.NoiNhan,
                SoKyHieu = x.SoKyHieu,
                SoTo = x.SoTo,
                Ten = x.Ten,
                TinhTrang = x.TinhTrang,
                TrangThai = x.TrangThai,
                TrichYeu = x.TrichYeu,
                User = x.User,
                UserId = x.UserId,
                NgayBanHanh = x.NgayBanHanh,
                HinhAnh = x.HinhAnh
            }).ToList();
        }
    }
}