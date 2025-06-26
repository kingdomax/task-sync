using Microsoft.Extensions.Options;

using TaskSync.Infrastructure.Settings;
using TaskSync.Models.View;
using TaskSync.Services.Interfaces;

namespace TaskSync.Services
{
    public class HomeVmService : IHomeVmService
    {
        private readonly AppInfo _appInfo;

        public HomeVmService(IOptions<AppInfo> options)
        {
            _appInfo = options.Value;
        }

        public HomeViewModel Build()
        {
            var vm = new HomeViewModel();

            vm.AppName = _appInfo.AppName;
            vm.Environment = _appInfo.Environment;

            return vm;
        }
    }
}
