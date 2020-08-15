using AutoMapper;
using DatingApp.API.DTO;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDTO>()
                .ForMember(
                    user => user.PhotoUrl, 
                    opt => opt.MapFrom(x => x.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(
                    user => user.Age, 
                    opt => opt.MapFrom(x => x.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDTO>()
                .ForMember(
                    user => user.PhotoUrl, 
                    opt => opt.MapFrom(x => x.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(
                    user => user.Age, 
                    opt => opt.MapFrom(x => x.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<Photo, PhotoForReturnDTO>();
            CreateMap<PhotoForCreationDTO, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDTO, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDTO>()
                .ForMember(x => x.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(x => x.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
