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


            CreateMap<Cvrc, CvrcDto>();
            CreateMap<CvrcDto, Cvrc>();
        }
    }

}
