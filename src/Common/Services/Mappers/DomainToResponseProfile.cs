using System.Linq;
using AutoMapper;
using Domain.Entities;
using Models.ResponseModels;

namespace Services.Mappers
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Animal, AnimalResponse>()
                .ForMember(
                    dest => dest.Appointments,
                    opt =>
                        opt.MapFrom(src => 
                            src.Appointments.Select(x =>
                                    new AppointmentResponse
                                    {
                                        Id = x.Id,
                                        Date = x.Date,
                                        Description = x.Description
                                    })
                    ));

            CreateMap<Appointment, AppointmentResponse>();
        }
    }
}
