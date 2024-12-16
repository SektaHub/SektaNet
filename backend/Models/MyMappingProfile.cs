using AutoMapper;
using backend.Models.Discord;
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

            CreateMap<DiscordChannelExport, DiscordChannelExportDto>();
            CreateMap<DiscordChannelExportDto, DiscordChannelExport>();

            CreateMap<Blogpost, BlogpostResponse>()
               .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher.UserName));

            CreateMap<BlogpostRequest, Blogpost>()
                .ForMember(dest => dest.Tags,
                           opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Tags)
                                                      ? new List<string>()
                                                      : src.Tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList()));

        }
    }

}
