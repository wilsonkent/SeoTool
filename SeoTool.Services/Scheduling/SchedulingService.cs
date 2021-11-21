using Quartz;
using Quartz.Impl;
using SeoTool.Services.Interface;
using System.Threading.Tasks;

namespace SeoTool.Services.Scheduling
{
    public class SchedulingService : ISchedulingService
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public SchedulingService()
        {
            _schedulerFactory = new StdSchedulerFactory();
        }

        public async Task StartSchedule(int secondsInterval, IExecutableModel model)
        {
            //start scheduler
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Start();

            //add executable job
            IJobDetail job = JobBuilder.Create<ExecutableJob>()
                .WithIdentity("job1", "group1")
                .Build();
            job.JobDataMap.Put("ExecutableModel", model);

            //add job trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(secondsInterval)
                    .RepeatForever())
                .Build();

            //schedule job with trigger
            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task StopSchedule()
        {
            //shut down scheduler
            var scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Shutdown();
        }
    }
}
