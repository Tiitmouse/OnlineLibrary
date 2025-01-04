using Data.Eunumerators;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Services;

public interface ILogService
{
    public Task Create(string message, Importance importance = Importance.None);
    public Task<List<Log>> GetPaginated(int n, int page);   
}

public class LogService : ILogService
{
    private readonly RwaContext _context;

    public LogService(RwaContext context)
    {
        _context = context;
    }

    public async Task Create(string message, Importance importance = Importance.None)
    {
        Log log = new Log
        {
            Date = DateTime.Now,
            Message = message,
            Importance = (int)importance
        };
        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Log>> GetPaginated(int n, int page)
    {
        return await _context.Logs
            .OrderByDescending(log => log.Date)
            .Skip(n * (page-1))
            .Take(n)
            .ToListAsync();
    }
}