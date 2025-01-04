using AutoMapper;
using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class LogController  : ControllerBase
{
    private readonly ILogService _logService;
    private readonly IMapper _mapper;

    public LogController(ILogService logService,IMapper mapper)
    {
        _logService = logService;
        _mapper = mapper;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateLog(LogDto dto)
    {
        await _logService.Create(dto.Message, dto.Importance);
        return Ok("Log created");
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchLogs(int n, int page)
    {
        var logs = await _logService.GetPaginated(n, page);
        var logDtos = _mapper.Map<List<LogDto>>(logs);
        return Ok(logDtos);
    }
}