using Data.Interfaces;
using Infrastructure.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Services.BackgroundWorkers
{
    public class ClearUnusedUserFiles : BackgroundService
    {
        private readonly int refreshRate;
        private readonly IServiceProvider serviceProvider;

        public ClearUnusedUserFiles(int refreshRate, IServiceProvider serviceProvider)
        {
            this.refreshRate = refreshRate;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = serviceProvider.CreateAsyncScope())
                {
                    var participantRepository = scope.ServiceProvider.GetRequiredService<IParticipantRepository>();
                    var filesProvider = scope.ServiceProvider.GetRequiredService<IFilesProvider>();                    

                    var filenames = await participantRepository.GetFilenamesAsync(cancellationToken);
                    filesProvider.DeleteUnusedUserFiles(filenames);
                }
                await Task.Delay(TimeSpan.FromHours(refreshRate), cancellationToken);
            }
        }
    }
}
