using AutoMapper;
using Data.Dto;
using Data.Models;

namespace WebApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<NewBookDto, Book>()
            .ForMember(dest => dest.Genres, opt => opt.Ignore())
            .ForMember(dest => dest.Locations, opt => opt.Ignore());
    }
    
}