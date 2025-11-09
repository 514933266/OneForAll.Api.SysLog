using SysLog.Host.Models;
using Quartz;
using SysLog.HttpService.Interfaces;
using System.Threading.Tasks;
using System;
using SysLog.Domain.Repositorys;
using System.Linq;
using SysLog.Application.Interfaces;
using SysLog.Domain.Models;
using NPOI.SS.Formula.Functions;

namespace SysLog.Host.QuartzJobs
{
    /// <summary>
    /// 删除日志
    /// </summary>
    public class DeleteLogJob : IJob
    {
        private readonly AuthConfig _config;
        private readonly ISysApiLogRepository _apiRepository;
        private readonly ISysExceptionLogRepository _exRepository;
        private readonly ISysLoginLogRepository _loginRepository;
        private readonly ISysOperationLogRepository _operaRepository;
        private readonly ISysGlobalExceptionLogRepository _gexRepository;

        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogService _gexService;
        public DeleteLogJob(
            AuthConfig config,
            ISysApiLogRepository apiRepository,
            ISysExceptionLogRepository exRepository,
            ISysLoginLogRepository loginRepository,
            ISysOperationLogRepository operaRepository,
            ISysGlobalExceptionLogRepository gexRepository,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogService gexService)
        {
            _config = config;
            _apiRepository = apiRepository;
            _exRepository = exRepository;
            _gexRepository = gexRepository;
            _loginRepository = loginRepository;
            _operaRepository = operaRepository;
            _jobHttpService = jobHttpService;
            _gexService = gexService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var apis = await _apiRepository.GetListAsync(w => w.CreateTime < DateTime.UtcNow.AddDays(-7));
                if (apis.Any())
                {
                    var num = await _apiRepository.DeleteRangeAsync(apis);
                    await AddLogAsync($"删除Api日志（7天前）任务执行完成，删除{num}条");
                }
                var exs = await _exRepository.GetListAsync(w => w.CreateTime < DateTime.UtcNow.AddMonths(-1));
                if (exs.Any())
                {
                    var num = await _exRepository.DeleteRangeAsync(exs);
                    await AddLogAsync($"删除异常日志（1个月前）任务执行完成，删除{num}条");
                }
                var gexs = await _gexRepository.GetListAsync(w => w.CreateTime < DateTime.UtcNow.AddMonths(-1));
                if (gexs.Any())
                {
                    var num = await _gexRepository.DeleteRangeAsync(gexs);
                    await AddLogAsync($"删除全局异常日志（1个月前）任务执行完成，删除{num}条");
                }
                var logs = await _loginRepository.GetListAsync(w => w.CreateTime < DateTime.UtcNow.AddMonths(-1));
                if (logs.Any())
                {
                    var num = await _loginRepository.DeleteRangeAsync(logs);
                    await AddLogAsync($"删除登录日志（1个月前）任务执行完成，删除{num}条");
                }
                var operas = await _operaRepository.GetListAsync(w => w.CreateTime < DateTime.UtcNow.AddMonths(-1));
                if (operas.Any())
                {
                    var num = await _operaRepository.DeleteRangeAsync(operas);
                    await AddLogAsync($"删除操作日志（1个月前）任务执行完成，删除{num}条");
                }
                await AddLogAsync($"删除日志任务执行完成");
            }
            catch (Exception ex)
            {
                await _gexService.AddAsync(new SysGlobalExceptionLogForm
                {
                    ModuleName = _config.ClientName,
                    ModuleCode = _config.ClientCode,
                    Name = ex.Message,
                    Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
                });
            }
        }

        // 添加日志
        private async Task AddLogAsync(string log)
        {
            await _jobHttpService.LogAsync(_config.ClientCode, typeof(DeleteLogJob).Name, log);
        }
    }
}
