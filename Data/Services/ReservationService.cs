using Data.Eunumerators;
using Data.Exceptions;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface IReservationService
{
    public Task<Reservation> Get(int id);
    public Task<Reservation> GetByBookId(int bookId);
    public Task Cancel(int id);
    public Task ChangeStatus(int id, bool newStatus);
    public Task<int> Reserve(Reservation newReservation);
    public Task<List<Reservation>> GetAll();
}

public class ReservationService : IReservationService
{
    private readonly RwaContext _context;
    private readonly ILogService _logService;
    
    public ReservationService(RwaContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }
    
    public async Task<Reservation> Get(int id)
    {
        Reservation reservation = await _context.Reservations
            .AsNoTracking()
            .Include(r => r.BookLocation)
            .Include(r => r.BookLocation.Book)
            .Include(r => r.BookLocation.Location)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.IdReservation == id);
        if (reservation == null)
        {
            await _logService.Create("Failed to fetch reservation with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Reservation with id {id} not found");
        }
        await _logService.Create($"Reservation with ID {id} fetched successfully", Importance.Low);
        return reservation;
    }

    public async Task<Reservation> GetByBookId(int bookId)
    {
        Reservation reservation = await _context.Reservations
            .AsNoTracking()
            .Include(r => r.BookLocation)
            .Include(r => r.BookLocation.Book)
            .Include(r => r.BookLocation.Location)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.BookLocation.BookId == bookId);
        if (reservation == null)
        {
            await _logService.Create("Failed to fetch reservation with book ID {bookId}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Reservation with book id {bookId} not found");
        }
          await _logService.Create($"Reservation with book ID {bookId} fetched successfully", Importance.Low);
          return reservation;
    }

    public async Task Cancel(int id)
    {
        Reservation reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.IdReservation == id);
        if (reservation == null)
        {
            await _logService.Create("Cannot delete reservation with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Reservation with id {id} not found");
        }
        _context.Reservations.Remove(reservation);
        await _logService.Create("Reservation with ID {id} successfully deleted", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeStatus(int id, bool newStatus)
    {
        Reservation original = await _context.Reservations.FirstOrDefaultAsync(r => r.IdReservation == id);
        if (original == null)
        {
            await _logService.Create("Cannot update reservation with ID {id}, because there is none with the same ID", Importance.Low);
            throw new NotFoundException($"Book with id {id} not found");
        }
        original.Status = newStatus;
        await _logService.Create("Reservation with ID {id} successfully updated", Importance.High);
        await _context.SaveChangesAsync();
    }

    public async Task<int> Reserve(Reservation newReservation)
    {
        Reservation? reservation = await _context.Reservations.FirstOrDefaultAsync(r => newReservation.BookLocation != null && r.BookLocation != null && r.BookLocation.BookId == newReservation.BookLocation.BookId);

        if (reservation != null)
        {
            await _logService.Create("Cannot reserve book with ID {reservation.BookLocation.BookId}, because it is already reserved", Importance.Low);
            if (newReservation.BookLocation != null)
                throw new AlreadyExistsException(
                    $"Reservation for book with ID {newReservation.BookLocation.BookId} already exists");
        }

        await _context.Reservations.AddAsync(newReservation);
        if (newReservation.BookLocation != null)
            if (newReservation.User != null)
                await _logService.Create(
                    $"Book with ID {newReservation.BookLocation.BookId} ({newReservation.BookLocation.Location.LocationName}) has been reserved by user {newReservation.User.IdUser}",
                    Importance.High);
        await _context.SaveChangesAsync();

        return _context.BookLocations
            .Include(bl=> bl.Book)
            .FirstOrDefault(bl=>bl.Id == newReservation.BookLocationId).Book.IdBook;
        
    }

    public async Task<List<Reservation>> GetAll()
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(r => r.BookLocation)
            .Include(r => r.BookLocation.Book)
            .Include(r => r.BookLocation.Location)
            .Include(r => r.User)
            .ToListAsync();
    }
}