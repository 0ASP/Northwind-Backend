using Core.CrossCutingConcerns.Caching;
using Core.CrossCutingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependenceResoulvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddMemoryCache();
            serviceDescriptors.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceDescriptors.AddSingleton<ICacheManager, MemoryCacheManager>();
        }
    }
}
