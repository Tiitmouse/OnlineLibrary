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

        CreateMap<Book, DetailsBookViewModel>()
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(b => b.Author.AuthorName))
            .ForMember(d => d.GenreName, opt => opt.MapFrom(b => b.Genre.GenreName))
            .ForMember(d => d.Libraries, opt => opt.MapFrom(b => b.BookLocations.Select(l => new LibraryAvailabilityViewModel
            {
                LocationId = l.Location.IdLocation,
                LocationName = l.Location.LocationName,
                LocationAddress = l.Location.Address,
                IsAvailable = !l.Reservations.Any(r => r.BookLocation.BookId == b.IdBook),
                BookLocationId = l.Id
            })));

        CreateMap<NewBookViewModel, Book>()
            .ForMember(b => b.IdBook, opt => opt.Ignore())
            .ForMember(b => b.Author, opt => opt.Ignore())
            .ForMember(b => b.Genre, opt => opt.Ignore())
            .ForMember(b => b.AuthorId, opt => opt.MapFrom(src => int.Parse(src.Author)))
            .ForMember(b => b.GenreId, opt => opt.MapFrom(src => int.Parse(src.Genre)));
        
        
        CreateMap<Genre, GenreViewModel>();
        CreateMap<GenreViewModel, Genre>();

        CreateMap<Location, LocationViewModel>();
        CreateMap<LocationViewModel, Location>();

        CreateMap<Author, AuthorViewModel>();
        CreateMap<AuthorViewModel, Author>();

        CreateMap<ReservationViewModel, Reservation>();

        CreateMap<BookLocation, LibraryAvailabilityViewModel>()
            .ForMember(l => l.LocationId, opt => opt.MapFrom(l => l.Location.IdLocation))
            .ForMember(l => l.LocationName, opt => opt.MapFrom(l => l.Location.LocationName))
            .ForMember(l => l.LocationAddress, opt => opt.MapFrom(l => l.Location.Address));
        CreateMap<LibraryAvailabilityViewModel, BookLocation>();



        CreateMap<Reservation, ReservationDetailsViewModel>()
            .ForMember(dest => dest.IdReservation, opt => opt.MapFrom(src => src.IdReservation))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CosumerName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.BookLocationId, opt => opt.MapFrom(src => src.BookLocationId))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookLocation.Book.Title))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.BookLocation.LocationId))
            .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        
        CreateMap<Reservation, UserReservation>()
            .ForMember(dest => dest.IdReservation, opt => opt.MapFrom(src => src.IdReservation))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.LibraryName, opt => opt.MapFrom(src => src.BookLocation.Location.LocationName))
            .ForMember(dest => dest.BookLocationId, opt => opt.MapFrom(src => src.BookLocationId))
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.BookLocation.Book.Title))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.BookLocation.LocationId))
            .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    
        
        CreateMap<User, UserDetailsViewModel>();
        CreateMap<UserDetailsViewModel, User>();


    }
}