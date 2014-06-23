using ReSharperToolKit.Modules;
using ReSharperToolKit.Modules.Services;
using ReTesterPlugin.Modules.Impl;

namespace ReTesterPlugin.Modules
{
    public static class Bootstrap
    {
        static Bootstrap()
        {
            Locator.Put<iAppTheme>(new AppTheme());
            Locator.Put<iTestProjectService>(new TestProjectService(Locator.Get<iNamingService>()));

            Locator.Put<iUnitTestService>(new UnitTestService(
                Locator.Get<iTestProjectService>(),
                Locator.Get<iProjectService>(),
                Locator.Get<iNamingService>()));
        }
    }
}