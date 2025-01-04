using Data.Eunumerators;

namespace Data.Dto;

public class LogDto
{
    public int IdLog { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; } 
    public Importance Importance { get; set; }
}