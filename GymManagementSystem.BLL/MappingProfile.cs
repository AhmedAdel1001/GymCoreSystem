using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.PlanViewModels;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Entities;
using System;

namespace GymManagementSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel))
                .ForMember(dest => dest.Photo, opt => opt.Ignore());

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();

            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? $"{src.Address.BuildingNumber} {src.Address.Street}, {src.Address.City}" : string.Empty))
                .ForMember(dest => dest.PlanName, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipStartDate, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipEndDate, opt => opt.Ignore());

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.BuildingNumber : 0))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : string.Empty))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : string.Empty));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (dest.Address == null) dest.Address = new Address();
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Trainer != null ? src.Trainer.Specialty.ToString() : string.Empty))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>()
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.ToString()));
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        }
    }
}

