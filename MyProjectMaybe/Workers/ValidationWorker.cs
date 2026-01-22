using MyProjectMaybe.Domain;
using MyProjectMaybe.Infrastructure;

namespace MyProjectMaybe.Workers
{
    public class ValidationWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ValidationWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ValidatePendingUploads(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
        
        private async Task ValidatePendingUploads(CancellationToken token)
        {
            using var scope = _scopeFactory.CreateScope();
            var store = scope.ServiceProvider.GetRequiredService<IUploadStore>();

            var pending = store.GetAll()
                .Where(u => u.State == UploadState.Created)
                .ToList();

            foreach (var upload in pending)
            {
                upload.State = UploadState.Validating;

                await Task.Delay(5000, token);

                upload.State = UploadState.Validated;
            }
        }

    }
}
