using AutoMapper;
using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Models;

namespace AwesomeDevEvents.API.Mappers
{
    public class DevEventProfile : Profile
    {

        public DevEventProfile() 
        {
            CreateMap<DevEvent, DevEventViewModel>();    // usado no GET
            CreateMap<DevEventSpeaker, DevEventSpeakerViewModel>();    // usado no GET

            CreateMap<DevEventInputModel, DevEvent>();   // usado no POST, PUT
            CreateMap<DevEventSpeakerInputModel, DevEventSpeaker>();   // usado no POST, PUT
        }

    }
}
