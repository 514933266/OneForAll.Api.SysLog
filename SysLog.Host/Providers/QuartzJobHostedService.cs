using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OneForAll.Core.Extension;
using Quartz;
using Quartz.Spi;
using SysLog.Application.Interfaces;
using SysLog.Domain.Models;
using SysLog.Host.Models;
using SysLog.HttpService.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SysLog.Host.Providers
{
    /// <summary>
    /// 定时任务启动服务
    /// </summary>
    public class QuartzJobHostedService : IHostedService
    {
        private readonly AuthConfig _authConfig;
        private readonly QuartzScheduleJobConfig _config;

        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ISysGlobalExceptionLogService _service;
        private readonly IScheduleJobHttpService _jobHttpService;

        public IScheduler Scheduler { get; private set; }
        public QuartzJobHostedService(
            AuthConfig authConfig,
            QuartzScheduleJobConfig config,
            IJobFactory jobFactory,
            ISchedulerFactory schedulerFactory,
            ISysGlobalExceptionLogService service,
            IScheduleJobHttpService jobHttpService)
        {
            _authConfig = authConfig;
            _config = config;
            _jobFactory = jobFactory;
            _schedulerFactory = schedulerFactory;

            _service = service;
            _jobHttpService = jobHttpService;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _config.ScheduleJobs)
            {
                if (jobSchedule.JobType == null)
                    continue;

                try
                {
                    // 尝试向调度中心注册
                    await _jobHttpService.RegisterAsync(new HttpService.Models.JobRegisterRequest()
                    {
                        AppId = _config.AppId,
                        AppSecret = _config.AppSecret,
                        GroupName = _config.GroupName,
                        NodeName = _config.NodeName,
                        Cron = jobSchedule.Cron,
                        Name = jobSchedule.TypeName,
                        Remark = jobSchedule.Remark
                    });
                }
                catch (Exception ex)
                {
                    // 防御性处理：注册过程异常也不应阻断本地任务
                    await _service.AddAsync(new SysGlobalExceptionLogForm()
                    {
                        ModuleName = _authConfig.ClientName,
                        ModuleCode = _authConfig.ClientCode,
                        Name = "定时任务注册异常",
                        Content = ex.StackTrace ?? "无堆栈信息"
                    });
                }

                // 无论注册是否成功，都创建并调度本地 Quartz 任务
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
            foreach (var jobSchedule in _config.ScheduleJobs)
            {
                await _jobHttpService.DownLineAsync(_config.AppId, jobSchedule.TypeName);
            }
        }

        // 创建任务
        private IJobDetail CreateJob(QuartzScheduleJob schedule)
        {
            return JobBuilder
                .Create(schedule.JobType)
                .WithIdentity(schedule.JobType.FullName)
                .WithDescription(schedule.JobType.Name)
                .Build();
        }

        // 创建触发器
        private ITrigger CreateTrigger(QuartzScheduleJob schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity(schedule.JobType.FullName.Append(".Trigger"))
                .WithCronSchedule(schedule.Cron)
                .WithDescription(schedule.JobType.Name.Append(".Trigger"))
                .Build();
        }
    }
}