using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Services.BackgroundWorkers
{
    public class ClearExpiredTokens : BackgroundService
    {
        private readonly int refreshRate;
        private readonly IServiceProvider serviceProvider;

        public ClearExpiredTokens(int refreshRate, IServiceProvider serviceProvider)
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
                    var tokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
                    await tokenRepository.DropExpiredAsync(cancellationToken);
                }
                await Task.Delay(TimeSpan.FromHours(refreshRate), cancellationToken);
            }
        }
    }
}
