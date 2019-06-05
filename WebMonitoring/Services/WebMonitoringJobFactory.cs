using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;

namespace WebMonitoring.Services
{
    public class WebMonitoringJobFactory : SimpleJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public WebMonitoringJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_serviceProvider.GetService(bundle.JobDetail.JobType);
        }
    }
}