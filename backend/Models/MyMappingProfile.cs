using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using System;

namespace backend.Models
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Reel, ReelDto>();
            CreateMap<ReelDto, Reel>();

            CreateMap<Image, ImageDto>();
            CreateMap<ImageDto, Image>();

            CreateMap<LongVideo, LongVideoDto>();
            CreateMap<LongVideoDto, LongVideo>();

            CreateMap<GenericFile, GenericFileDto>();
            CreateMap<GenericFileDto, GenericFile>();

            CreateMap<Audio, AudioDto>();
            CreateMap<AudioDto, Audio>();
        }
    }

}
