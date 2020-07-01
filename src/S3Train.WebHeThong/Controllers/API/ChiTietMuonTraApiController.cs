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
    public class ChiTietMuonTraApiController : ApiController
    {
        private readonly IChiTietMuonTraService _chiTietMuonTraService;

        public ChiTietMuonTraApiController()
        {

        }

        public ChiTietMuonTraApiController(IChiTietMuonTraService chiTietMuonTraService)
        {
            _chiTietMuonTraService = chiTietMuonTraService;
        }

        public IHttpActionResult GetAll()
        {
            var list = _chiTietMuonTraService.GetAll().ToList().Select(Mapper.Map<ChiTietMuonTra, ChiTietMuonTraDto>);
            return Ok(list);
        }

        public IHttpActionResult GetByMuonTraId(string muonTraId)
        {
            if (string.IsNullOrEmpty(muonTraId))
                return BadRequest();

            var chiTietMuonTras = _chiTietMuonTraService.Gets(p => p.MuonTraID == muonTraId && p.TrangThai == true).ToList().Select(Mapper.Map<ChiTietMuonTra, ChiTietMuonTraDto>);

            if (chiTietMuonTras == null)
                return NotFound();
            return Ok(chiTietMuonTras);
        }

        public IHttpActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var chiTietMuonTra = _chiTietMuonTraService.Get(p => p.Id == id);

            if (chiTietMuonTra == null)
                return NotFound();
            return Ok(Mapper.Map<ChiTietMuonTra, ChiTietMuonTraDto>(chiTietMuonTra));
        }
    }
}
