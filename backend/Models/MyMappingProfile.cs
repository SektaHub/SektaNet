using AutoMapper;
using backend.Models.Dto;
using backend.Models.Entity;
using System;
using backend.Models.Common;

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
