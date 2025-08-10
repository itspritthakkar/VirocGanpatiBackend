using AutoMapper;
using VirocGanpati.DTOs;
using VirocGanpati.DTOs.ArtiEveningTime;
using VirocGanpati.DTOs.ArtiMorningTimes;
using VirocGanpati.DTOs.DateOfVisarjans;
using VirocGanpati.DTOs.MandalAreas;
using VirocGanpati.Models;

namespace VirocGanpati.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Mandal, MandalDto>();
            CreateMap<AddMandalDto, Mandal>();
            CreateMap<UpdateMandalDto, Mandal>();
            CreateMap<Record, RecordDto>().ForMember(des => des.UpdatedBy, opt => opt.MapFrom(src => src.Updater != null ? $"{src.Updater.FirstName} {src.Updater.LastName}" : string.Empty));
            CreateMap<AddRecordDto, Record>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.RoleIdentifier, opt => opt.MapFrom(src => src.Role.RoleIdentifier))
                .ForMember(dest => dest.MandalName, opt => opt.MapFrom(src => src.Mandal.MandalName))
                .ForMember(dest => dest.MandalSlug, opt => opt.MapFrom(src => src.Mandal.MandalSlug));
            CreateMap<Role, RoleDto>();
            CreateMap<AddRoleDto, Role>();
            CreateMap<AddDocumentsDto, Document>()
           .ForMember(dest => dest.FileName, opt => opt.Ignore())
           .ForMember(dest => dest.Extension, opt => opt.Ignore());
            CreateMap<Document, DocumentDto>();
            CreateMap<DocumentDto, Document>();
            CreateMap<MandalArea, MandalAreaDto>();
            CreateMap<CreateMandalAreaDto, MandalArea>();
            CreateMap<UpdateMandalAreaDto, MandalArea>();
            CreateMap<ArtiMorningTime, ArtiMorningTimeDto>();
            CreateMap<CreateArtiMorningTimeDto, ArtiMorningTime>();
            CreateMap<UpdateArtiMorningTimeDto, ArtiMorningTime>();
            CreateMap<ArtiEveningTime, ArtiEveningTimeDto>();
            CreateMap<CreateArtiEveningTimeDto, ArtiEveningTime>();
            CreateMap<UpdateArtiEveningTimeDto, ArtiEveningTime>();
            CreateMap<DateOfVisarjan, DateOfVisarjanDto>();
            CreateMap<CreateDateOfVisarjanDto, DateOfVisarjan>();
            CreateMap<UpdateDateOfVisarjanDto, DateOfVisarjan>();
        }
    }
}