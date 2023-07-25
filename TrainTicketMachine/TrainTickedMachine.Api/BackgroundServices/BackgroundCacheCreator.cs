using TrainTickedMachine.Api.Services;

namespace TrainTickedMachine.Api.BackgroundServices
{
    public class BackgroundCacheCreator : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BackgroundCacheCreator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    cacheService.SetCacheFromApi();
                }

                //wait 30 minutes before updating the cache again
                //this is to avoid hitting the central system too often
                //and to avoid hitting the cache too often
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
