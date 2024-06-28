using AutoMapper;
using Truck_Visit_Management.Dtos;
using Truck_Visit_Management.Entities;

namespace Truck_Visit_Management.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VisitRecordEntity, VisitRecordDto>().ReverseMap();
            CreateMap<ActivityEntity, ActivityDto>().ReverseMap();
            CreateMap<DriverInformationEntity, DriverInformationDto>().ReverseMap();
        }
    }

}
