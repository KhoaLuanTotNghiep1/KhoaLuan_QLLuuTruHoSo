using AutoMapper;
using S3Train.Domain;
using S3Train.Model;
using S3Train.Model.Dto;
using S3Train.Model.User;

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
    
            Mapper.CreateMap<ApplicationUser, UserViewModel>();
            Mapper.CreateMap<UserViewModel, ApplicationUser>();
        }
    }
}