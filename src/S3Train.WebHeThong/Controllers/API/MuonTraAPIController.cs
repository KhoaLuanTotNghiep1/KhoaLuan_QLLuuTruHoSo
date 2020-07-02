using AutoMapper;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace S3Train.WebHeThong.Controllers.API
{
    public class MuonTraAPIController : ApiController
    {
        private readonly IMuonTraService _muonTraService;

        public MuonTraAPIController()
        {

        }

        public MuonTraAPIController(IMuonTraService muonTraService)
        {
            _muonTraService = muonTraService;
        }

        public IHttpActionResult GetAll()
        {
            var list = _muonTraService.GetAll().ToList().Select(Mapper.Map<MuonTra, MuonTraDto>);
            return Ok(list);
        }

        public IHttpActionResult GetByUserId(string userId, int tinhTrang)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();
            EnumTinhTrang enumTinh = (EnumTinhTrang)tinhTrang;
            var muontra = _muonTraService.Gets(p => p.UserId == userId && p.TinhTrang == enumTinh && p.TrangThai == true).ToList().Select(Mapper.Map<MuonTra, MuonTraDto>);

            if (muontra == null)
                return NotFound();
            return Ok(muontra);
        }

        public IHttpActionResult GetByTinhTrang(int tinhTrang)
        {
            EnumTinhTrang enumTinh = (EnumTinhTrang)tinhTrang;
            var muontra = _muonTraService.Gets(p => p.TinhTrang == enumTinh && p.TrangThai == true).ToList().Select(Mapper.Map<MuonTra, MuonTraDto>);

            if (muontra == null)
                return NotFound();
            return Ok(muontra);
        }

        public IHttpActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var muontra = Mapper.Map<MuonTra, MuonTraDto>(_muonTraService.Get(p => p.Id == id));

            if (muontra == null)
                return NotFound();
            return Ok(muontra);
        }
    }
}
