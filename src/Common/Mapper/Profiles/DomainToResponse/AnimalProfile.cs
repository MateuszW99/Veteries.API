using AutoMapper;
using Domain.Entities;
using Models.ResponseModels;

namespace Mapper.Profiles.DomainToResponse
{
    public class AnimalProfile : Profile
    {
        public AnimalProfile()
        {
            CreateMap<Animal, AnimalResponse>();
            CreateMap<Appointment, AppointmentResponse>();
        }
    }
}
