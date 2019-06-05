using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Threading;
using System.Threading.Tasks;

namespace WebMonitoring.Services
{
    public class WebMonitoringHostedService : IHostedService
    {
        private readonly IJobFactory _jobFactory;
        private IScheduler _scheduler;

        public WebMonitoringHostedService(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            _scheduler.JobFactory = _jobFactory;
            await _scheduler.Start();
            var job = JobBuilder.Create<WebMonitoringJob>().Build();
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(scheduleBuilder => scheduleBuilder.WithIntervalInHours(1).RepeatForever())
                .Build();
            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler?.Shutdown(cancellationToken);
        }
    }
}