using AutoMapper;
using Data.Dto;
using Data.Models;
using WebApp.Models;

namespace WebApp;

public class MapperProfile : Profile
{
    public MapperProfile ()
    {
        CreateMap<Book, BookViewModel>()
            .ForMember(b => b.AuthorName, opt => opt.MapFrom(b => b.Author.AuthorName))
            .ForMember(b => b.GenreName, opt => opt.MapFrom(b => b.Genre.GenreName));
        CreateMap<BookViewModel, Book>();
        
        CreateMap<Genre, GenreViewModel>();
        CreateMap<GenreViewModel, Genre>();
        
        CreateMap<Author, AuthorViewModel>();
        CreateMap<AuthorViewModel, Author>();


    }
}