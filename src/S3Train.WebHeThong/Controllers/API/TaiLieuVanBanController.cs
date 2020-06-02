using AutoMapper;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model;
using S3Train.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace S3Train.WebHeThong.Controllers.API
{
    public class TaiLieuVanBanController : ApiController
    {
        private readonly ITaiLieuVanBanService _taiLieuVanBanService;

        public TaiLieuVanBanController()
        {

        }

        public TaiLieuVanBanController(ITaiLieuVanBanService taiLieuVanBanService)
        {
            _taiLieuVanBanService = taiLieuVanBanService;
        }

        public IEnumerable<TaiLieuVanBanDto> GetAll()
        {
            var taiLieuVanBanDtos = _taiLieuVanBanService.Gets(p => p.TrangThai == true, p => p.OrderByDescending(c => c.NgayTao))
                  .ToList().Select(Mapper.Map<TaiLieuVanBan, TaiLieuVanBanDto>);

            return taiLieuVanBanDtos;
        }

        [ResponseType(typeof(NoiBanHanhDto))]
        public IHttpActionResult GetBysearchString(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return BadRequest();

            var taiLieuVanBanDtos = _taiLieuVanBanService.Gets(p => p.TrangThai == true && p.Ten.Contains(searchString) || p.NoiDung.Contains(searchString),
                p => p.OrderBy(c => c.NgayTao)).ToList().Select(Mapper.Map<TaiLieuVanBan, TaiLieuVanBanDto>);

            if (taiLieuVanBanDtos == null)
                return NotFound();

            return Ok(taiLieuVanBanDtos);
        }

        //[ResponseType(typeof(NoiBanHanhDto))]
        //public IHttpActionResult GetByDang(EnumDangVanBan dang)
        //{
        //    if (dang != null)
        //        return BadRequest();

        //    var taiLieuVanBanDtos = _taiLieuVanBanService.Gets(p => p.TrangThai == true && p.Dang == dang,
        //        p => p.OrderBy(c => c.NgayTao)).ToList().Select(Mapper.Map<TaiLieuVanBan, TaiLieuVanBanDto>);

        //    if (taiLieuVanBanDtos == null)
        //        return NotFound();

        //    return Ok(taiLieuVanBanDtos);
        //}

        [ResponseType(typeof(NoiBanHanhDto))]
        public IHttpActionResult GetByTime(DateTime? startTime, DateTime? endTime)
        {
            var taiLieuVanBans = _taiLieuVanBanService.Gets(p => p.TrangThai == true, p => p.OrderByDescending(c => c.NgayTao));

            if (startTime.HasValue)
                taiLieuVanBans = taiLieuVanBans.Where(p => p.NgayTao >= startTime).ToList();

            if (endTime.HasValue)
                taiLieuVanBans = taiLieuVanBans.Where(p => p.NgayTao <= endTime).ToList();

            if (taiLieuVanBans == null)
                return NotFound();

            return Ok(taiLieuVanBans.Select(Mapper.Map<TaiLieuVanBan, TaiLieuVanBanDto>));
        }
    }
}
