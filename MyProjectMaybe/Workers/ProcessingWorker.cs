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
                upload.State = UploadState.Processing;

                try
                {
                    // Simulate irreversible work
                    await Task.Delay(1000, token);

                    upload.State = UploadState.Completed;
                }
                catch
                {
                    upload.State = UploadState.Failed;
                }
            }

        }

    }
}
