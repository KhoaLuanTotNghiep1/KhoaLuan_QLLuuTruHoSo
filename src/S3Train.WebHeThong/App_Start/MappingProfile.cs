using AutoMapper;
using S3Train.Domain;
using S3Train.Model;
using S3Train.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<NoiBanHanh, NoiBanHanhDto>();
            Mapper.CreateMap<NoiBanHanhDto, NoiBanHanh>();

            Mapper.CreateMap<TaiLieuVanBan, TaiLieuVanBanDto>();
            Mapper.CreateMap<TaiLieuVanBanDto, TaiLieuVanBan>();
        }
    }
}