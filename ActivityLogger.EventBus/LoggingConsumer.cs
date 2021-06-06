using System;
using System.Threading.Tasks;
using ActivityLogger.Persistence;
using ActivityLogger.Persistence.Models;
using CyberCats_Bank.EventBus.Contracts.Logging;
using MassTransit;

namespace ActivityLogger.EventBus
{
    public class LoggingConsumer : IConsumer<LogContract>
    {
        private readonly AppDbContext dbContext;

        public LoggingConsumer(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<LogContract> context)
        {
            var log = new Log()
            {
                Action = context.Message.Action,
                Application = context.Message.Application,
                DateCreated = DateTime.Now
            };
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }
}