using AutoMapper;
using Data.Dto;
using Data.Models;

namespace WebApi;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<NewBookDto, Book>();
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.AuthorName))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.GenreName));
        CreateMap<BookDto, Book>();
        CreateMap<Book, BookDetailsDto>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.BookLocations.Select(bl => bl.Location)));
        CreateMap<BookDetailsDto, Book>();

        CreateMap<Genre, GenreDto>();
        CreateMap<GenreDto, Genre>()
            .ForMember(dest => dest.IdGenre, opt => opt.Ignore());

        CreateMap<Location, LocationDto>();
        CreateMap<LocationDto, Location>()
            .ForMember(dest => dest.IdLocation, opt => opt.Ignore());

        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, Author>()
            .ForMember(dest => dest.IdAuthor, opt => opt.Ignore());
    }
    
}