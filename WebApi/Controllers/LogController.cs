using AutoMapper;
using Data.Dto;
using Data.Eunumerators;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
public class LogController  : ControllerBase
{
    private readonly ILogService _logService;
    private readonly IMapper _mapper;

    public LogController(ILogService logService,IMapper mapper)
    {
        _logService = logService;
        _mapper = mapper;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchLogs(int n, int page)
    {
        var logs = await _logService.GetPaginated(n, page);
        var logDtos = _mapper.Map<List<LogDto>>(logs);
        return Ok(logDtos);
    }
}