using AutoMapper;
using Data.Dto;
using Data.Models;

namespace WebApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<NewBookDto, Book>();

    }
    
}