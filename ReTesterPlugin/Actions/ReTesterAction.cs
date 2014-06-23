using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using ReSharperToolKit.Actions;
using ReSharperToolKit.Modules;
using ReTesterPlugin.Modules;

namespace ReTesterPlugin.Actions
{
    public abstract class ReTesterAction : ClassActionBase
    {
        /// <summary>
        /// Services related to the target test project.
        /// </summary>
        protected readonly iTestProjectService TestProjectService;

        /// <summary>
        /// The current theme
        /// </summary>
        protected readonly iAppTheme Theme;

        /// <summary>
        /// Services related to the unit test.
        /// </summary>
        protected readonly iUnitTestService UnitTestService;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ReTesterAction(ICSharpContextActionDataProvider pProvider)
            : base(pProvider)
        {
            Theme = Locator.Get<iAppTheme>();
            UnitTestService = Locator.Get<iUnitTestService>();
            TestProjectService = Locator.Get<iTestProjectService>();
        }
    }
}