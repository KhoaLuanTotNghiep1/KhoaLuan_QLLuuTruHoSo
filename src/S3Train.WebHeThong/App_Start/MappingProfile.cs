using AutoMapper;
using S3Train.Domain;
using S3Train.Model;
using S3Train.Model.Dto;
using S3Train.Model.User;
using System.Collections.Generic;

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

            Mapper.CreateMap<HoSo, HoSoDto>();
            Mapper.CreateMap<HoSoDto, HoSo>();

            Mapper.CreateMap<Hop, HopDto>();
            Mapper.CreateMap<HopDto, Hop>();

            Mapper.CreateMap<Ke, KeDto>();
            Mapper.CreateMap<KeDto, Ke>();

            Mapper.CreateMap<Tu, TuDto>();
            Mapper.CreateMap<TuDto, Tu>();

            Mapper.CreateMap<ChiTietMuonTra, ChiTietMuonTraDto>();
            Mapper.CreateMap<ChiTietMuonTraDto, ChiTietMuonTra>();

            Mapper.CreateMap<MuonTra, MuonTraDto>();
            Mapper.CreateMap<MuonTraDto, MuonTra>();

            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<UserDto, ApplicationUser>();
        }
    }
}