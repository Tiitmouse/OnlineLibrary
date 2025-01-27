using AutoMapper;
using Data.Dto;
using Data.Eunumerators;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
public class LogController  : ControllerBase
{
    private readonly ILogService _logService;
    private readonly IMapper _mapper;

    public LogController(ILogService logService,IMapper mapper)
    {
        _logService = logService;
        _mapper = mapper;
    }
    //TODO: mby rename
    [HttpGet("[action]")]
    public async Task<IActionResult> FetchLogs([FromQuery] int n, int page)
    {
        var logs = await _logService.GetPaginated(n, page);
        var logDtos = _mapper.Map<List<LogDto>>(logs);
        return Ok(logDtos);
    }
    
    //TODO: Create route /get/N where N is number of logs to fetch default n is 10
    
    [HttpGet("[action]")]
    public async Task<int> Count()
    {
        return await _logService.CountLogs();
    }
}