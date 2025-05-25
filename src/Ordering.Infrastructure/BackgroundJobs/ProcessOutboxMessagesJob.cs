using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Core.Abstraction;
using Ordering.Core.Aggregate.OutboxMessageAggregate;
using Quartz;

namespace Ordering.Infrastructure.BackgroundJobs;

/// <summary>
/// To process outbox messages every 5 seconds.
/// </summary>
/// <param name="outboxMessageRepository">
/// To get outbox messages to execute.
/// </param>
/// <param name="logger">
/// Logger for logging information and errors.
/// </param>
public class ProcessOutboxMessagesJob(
    IOutboxMessageRepository outboxMessageRepository,
    ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private const int BatchSize = 10;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger = logger;
    private readonly IOutboxMessageRepository _outboxMessageRepository = outboxMessageRepository;
    
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Processing outbox messages every 5 seconds... ");
        
        var outboxMessages = await _outboxMessageRepository
            .GetOutboxMessageToExecute(BatchSize, context.CancellationToken);

        foreach (var outboxMessage in outboxMessages)
        {
            try
            {
                var domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(outboxMessage.Content,new JsonSerializerSettings 
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })!;

                // Todo: publish event here
                
            }
            catch (Exception exception)
            {
                outboxMessage.Error = exception.Message;
            }
            
            outboxMessage.ProcessedAt = DateTime.UtcNow;
        }

        await _outboxMessageRepository.UnitOfWork.SaveChangesAsync();
    }
}