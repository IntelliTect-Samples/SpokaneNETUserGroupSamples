using System.Collections.Concurrent;

namespace PixelController.Api.Models;

public class LongRunningService : BackgroundService
{
    private readonly BackgroundWorkerQueue queue;

    public LongRunningService(BackgroundWorkerQueue queue)
    {
        this.queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await queue.DequeueAsync(stoppingToken);

            await workItem(stoppingToken);
        }
    }
}