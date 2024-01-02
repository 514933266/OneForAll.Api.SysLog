using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.Extension;
using SysLog.Domain.AggregateRoots;
using SysLog.Domain.Enums;
using SysLog.Domain.Interfaces;
using SysLog.Domain.Repositorys;
using SysLog.HttpService.Interfaces;
using SysLog.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.Domain
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class SysNotificationManager : SysBaseManager, ISysNotificationManager
    {
        private readonly ISysNotificationConfigRepository _repository;
        private readonly ISysUmsMessageHttpService _umsHttpService;

        public SysNotificationManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ISysNotificationConfigRepository repository,
            ISysUmsMessageHttpService umsHttpService) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
            _umsHttpService = umsHttpService;
        }
    }
}
