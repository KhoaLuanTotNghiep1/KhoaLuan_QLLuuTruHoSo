using AutoMapper;
using S3Train.Contract;
using S3Train.Domain;
using S3Train.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace S3Train.WebHeThong.Controllers.API
{
    public class TestController : ApiController
    {
        private readonly INoiBanHanhService _noiBanHanhService;

        public TestController()
        {

        }

        public TestController(INoiBanHanhService noiBanHanhService)
        {
            _noiBanHanhService = noiBanHanhService;
        }

        public IEnumerable<NoiBanHanhDto> Get()
        {
            var a = _noiBanHanhService.Gets(p => p.TrangThai == true, p => p.OrderBy(c => c.NgayTao))
                .ToList().Select(Mapper.Map<NoiBanHanh, NoiBanHanhDto>);
            return a;
        }

        [ResponseType(typeof(NoiBanHanhDto))]
        public IHttpActionResult GetById(string id)
        {
            var a = Mapper.Map<NoiBanHanh, NoiBanHanhDto>(_noiBanHanhService.GetById(id));

            if (a == null)
                return NotFound();

            return Ok(a);
        }

        [HttpPost]
        [ResponseType(typeof(NoiBanHanhDto))]
        public IHttpActionResult Create(NoiBanHanhDto noiBanHanhDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var noi = Mapper.Map<NoiBanHanhDto, NoiBanHanh>(noiBanHanhDto);

            _noiBanHanhService.Insert(noi);
            noiBanHanhDto.Id = noi.Id;

            return Ok(noiBanHanhDto);
        }

        [HttpPut]
        [ResponseType(typeof(NoiBanHanhDto))]
        public IHttpActionResult Update(string id,NoiBanHanhDto noiBanHanhDto)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return BadRequest();

            var noi = _noiBanHanhService.GetById(id);

            if (noi == null)
                return NotFound();

            var a = Mapper.Map(noiBanHanhDto, noi);
            _noiBanHanhService.Update(a);

            return Ok(noiBanHanhDto);
        }

        [HttpDelete]
        [ResponseType(typeof(NoiBanHanhDto))]
        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var noi = _noiBanHanhService.GetById(id);

            if (noi == null)
                return NotFound();

            _noiBanHanhService.Remove(noi);

            return Ok(Mapper.Map<NoiBanHanh, NoiBanHanhDto>(noi));
        }
    }
}
