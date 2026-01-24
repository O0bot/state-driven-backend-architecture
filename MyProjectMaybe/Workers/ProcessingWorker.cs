using MyProjectMaybe.Domain;
using MyProjectMaybe.Infrastructure;

namespace MyProjectMaybe.Workers
{
    public class ProcessingWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        
        public ProcessingWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessValidateUploads(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }

        private async Task ProcessValidateUploads(CancellationToken token)
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IUploadStore>();

            var pending = store.GetAll()
                .Where(u => u.State == UploadState.Validated)
                .ToList();

            foreach (var upload in pending)
            {
                // If side effects already happened, just finalize
                if (upload.IsProcessed)
                {
                    upload.State = UploadState.Completed;
                    continue;
                }

                upload.State = UploadState.Processing;

                try
                {
                    // Simulate irreversible work
                    await Task.Delay(1000, token);

                    upload.IsProcessed = true;
                    upload.State = UploadState.Completed;
                }
                catch
                {
                    // Do NOTHING
                    // Leave state as Processing so lease logic can retry
                }
            }

        }

    }
}
