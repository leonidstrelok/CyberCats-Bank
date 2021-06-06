using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ActivityLogger.Dtos;
using ActivityLogger.Persistence;
using ActivityLogger.Persistence.Models.Enums;
using Serilog;
using Log = ActivityLogger.Persistence.Models.Log;

namespace ActivityLogger.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly AppDbContext dbContext;

        public LoggerController(ILogger logger, AppDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        [HttpPost("loggingInformation")]
        public async Task<IActionResult> LoggingInformation([FromBody] LogDTO logDto)
        {
            Log log = new()
            {
                DateCreated = DateTime.Now,
                Application = logDto.Application,
                LoggingType = LoggingType.Error
            };
            logger.Information(log.LoggingType.ToString(), log.DateCreated.ToString(), logDto.Application);
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("LoggingError")]
        public async Task<IActionResult> LoggingError([FromBody] LogDTO logDto)
        {
            Log log = new()
            {
                DateCreated = DateTime.Now,
                Application = logDto.Application,
                LoggingType = LoggingType.Error
            };
            logger.Warning(log.LoggingType.ToString(), log.DateCreated.ToString(), logDto.Application);
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("LoggingWarning")]
        public async Task<IActionResult> LoggingWarning([FromBody] LogDTO logDto)
        {
            Log log = new()
            {
                DateCreated = DateTime.Now,
                Application = logDto.Application,
                LoggingType = LoggingType.Error
            };
            logger.Error(log.LoggingType.ToString(), log.DateCreated.ToString(), logDto.Application);

            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}