using AutoMapper;
using Domain.Entities;
using Models.ResponseModels;

namespace Veteries.API.Profiles
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
