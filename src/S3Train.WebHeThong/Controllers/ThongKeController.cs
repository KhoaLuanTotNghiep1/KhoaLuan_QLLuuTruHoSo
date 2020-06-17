using Newtonsoft.Json;
using S3Train.Contract;
using S3Train.Core.Constant;
using S3Train.Core.Extension;
using S3Train.Domain;
using S3Train.WebHeThong.CommomClientSide.Function;
using S3Train.WebHeThong.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace S3Train.WebHeThong.Controllers
{
    [Authorize(Roles = GlobalConfigs.ROLE_GIAMDOC_CANBOVANTHU)]
    public class ThongKeController : Controller
    {
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;
        private readonly IMuonTraService _muonTraService;
        private readonly IChiTietMuonTraService _chiTietMuonTraService;

        public ThongKeController()
        {

        }

        public ThongKeController(ITaiLieuVanBanService taiLieuVanBanService, IMuonTraService muonTraService,
            IChiTietMuonTraService chiTietMuonTraService)
        {
            _taiLieuVanBanService = taiLieuVanBanService;
            _muonTraService = muonTraService;
            _chiTietMuonTraService = chiTietMuonTraService;
        }

        // GET: ThongKe
        public ActionResult Index(DateTime? startTime, DateTime? endTime)
        {
            var list = GetDoc(startTime, endTime);

            HttpContext.Session["ListTK"] = list;

            var dataPoints = AddList.ListDataPonit(list);

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(list);
        }

        public ActionResult MuonTra(DateTime? startTime, DateTime? endTime)
        {
            var list = GetMuonTra(startTime, endTime);

            HttpContext.Session["ListMT"] = list;
            ViewBag.UsersBorrowDocument = GetUsersBorrowDocument();
            var dataPoints = AddList.ListDataPonit(list);

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(list);
        }

        public List<MuonTra> GetUsersBorrowDocument()
        {
            var list = _muonTraService.GetAllHaveJoinUser();
            var users = list.Where(p => p.NgayKetThuc < DateTime.Now && p.TinhTrang == EnumTinhTrang.DangMuon && p.TrangThai == true);

            return users.ToList();
        }


        public void Export(string type, string dang)
        {
            var dictionary = HttpContext.Session["ListTK"] as Dictionary<string, List<TaiLieuVanBan>>;

            var taiLieuVanBans = new List<TaiLieuVanBan>();

            if (string.IsNullOrEmpty(dang))
            {
                foreach(var item in dictionary)
                    taiLieuVanBans.AddRange(item.Value);
            }
            else
                taiLieuVanBans = dictionary.FirstOrDefault(p => p.Key == dang).Value;

            if (type == "excel")
            {
                ExportFileExel(taiLieuVanBans);
            }
            else
            {
                ExportFileCSV(taiLieuVanBans);
            }
        }

        public void ExportMuontra(string type, string dang)
        {
            var dictionary = HttpContext.Session["ListMT"] as Dictionary<string, List<ChiTietMuonTra>>;

            var chiTietMuonTras = new List<ChiTietMuonTra>();

            if (string.IsNullOrEmpty(dang))
            {
                foreach (var item in dictionary)
                    chiTietMuonTras.AddRange(item.Value);
            }
            else
                chiTietMuonTras = dictionary.FirstOrDefault(p => p.Key == dang).Value;

            if (type == "excel")
                ExportFileExelMuonTra(chiTietMuonTras);
            else
                ExportFileCSVMuonTra(chiTietMuonTras);
        }

        public void ExportFileCSV(List<TaiLieuVanBan> taiLieuVanBans)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0},{1},{2},{3},{4},{5}", "Số ký hiệu", "Tên", "Loại ", "Dạng", "Tình Trạng", Environment.NewLine);
            foreach (var item in taiLieuVanBans)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4},{5}", item.SoKyHieu, item.Ten, item.Loai,item.Dang, item.TinhTrang.GetDecription(), Environment.NewLine);
            }

            //Get Current Response  
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=Employee.CSV ");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }

        public void ExportFileExel(List<TaiLieuVanBan> taiLieuVanBans)
        {
            var grid = new GridView
            {
                DataSource = from tl in taiLieuVanBans
                             select new { tl.SoKyHieu, tl.Ten, tl.Loai, tl.Dang, tinhTrang = tl.TinhTrang.GetDecription() }
            };

            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("Content-disposition", "attachment;filename=ThongKeExcel.xls");
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.End();
        }

        public void ExportFileCSVMuonTra(List<ChiTietMuonTra> chiTietMuonTras)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0},{1},{2}", "Tên Văn Bản", "Người Mượn","Dạng", "Ngày Tạo", Environment.NewLine);
            foreach (var item in chiTietMuonTras)
            {
                sb.AppendFormat("{0},{1},{2}" ,item.TaiLieuVanBan.Ten, item.MuonTra.User.FullName, item.TrangThai == true ? "Trả" : "Mượn" ,
                    item.NgayTao, Environment.NewLine);
            }

            //Get Current Response  
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=Employee.CSV ");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }

        public void ExportFileExelMuonTra(List<ChiTietMuonTra> chiTietMuonTras)
        {
            var grid = new GridView
            {
                DataSource = from tl in chiTietMuonTras
                             select new { tl.TaiLieuVanBan.Ten, tl.MuonTra.User.FullName,dang = tl.TrangThai == true ? "Trả" : "Mượn", tl.NgayTao }
            };

            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("Content-disposition", "attachment;filename=ThongKeExcel.xls");
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.End();
        }

        private Dictionary<string, List<TaiLieuVanBan>> GetDoc(DateTime? startTime, DateTime? endTime)
        {
            var list = new Dictionary<string, List<TaiLieuVanBan>>();

            var taiLieuVanBans = _taiLieuVanBanService.GetAll();

            if (startTime.HasValue)
            {
                taiLieuVanBans = taiLieuVanBans.Where(p => p.NgayTao >= startTime).ToList();
            }              

            if (endTime.HasValue)
            {
                taiLieuVanBans = taiLieuVanBans.Where(p => p.NgayTao <= endTime).ToList();
            }              

            var a = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_DEN).ToList();
            var b = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_DI).ToList();
            var c = taiLieuVanBans.Where(p => p.Dang == GlobalConfigs.DANG_NOIBO).ToList();

            list.Add(GlobalConfigs.DANG_DEN, a);
            list.Add(GlobalConfigs.DANG_DI, b);
            list.Add(GlobalConfigs.DANG_NOIBO, c);

            return list;
        }

        private Dictionary<string, List<ChiTietMuonTra>> GetMuonTra(DateTime? startTime, DateTime? endTime)
        {
            var list = new Dictionary<string, List<ChiTietMuonTra>>();

            var chiTietMuonTras = _chiTietMuonTraService.GetAllHaveJoinTLVB();

            if (startTime.HasValue)
            {
                chiTietMuonTras = chiTietMuonTras.Where(p => p.NgayTao >= startTime);
            }

            if (endTime.HasValue)
            {
                chiTietMuonTras = chiTietMuonTras.Where(p => p.NgayTao <= endTime);
            }

            var a = chiTietMuonTras.Where(p => p.TrangThai == true).ToList();
            var b = chiTietMuonTras.Where(p => p.TrangThai == false).ToList();

            list.Add("Danh Sách Văn Bản Mượn", a);
            list.Add("Danh Sách Văn Bản Trả", b);

            return list;
        }

    }
}