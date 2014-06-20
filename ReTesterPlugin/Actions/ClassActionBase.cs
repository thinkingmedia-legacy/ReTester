using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReTesterPlugin.Exceptions;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions
{
    public abstract class ClassActionBase : ContextActionBase
    {
        /// <summary>
        /// Naming convention service
        /// </summary>
        protected readonly iNamingService NamingService;

        /// <summary>
        /// Not null if editing a C# file.
        /// </summary>
        protected readonly ICSharpContextActionDataProvider Provider;

        /// <summary>
        /// Services related to the target test project.
        /// </summary>
        protected readonly iTestProjectService TestProjectService;

        /// <summary>
        /// The current theme
        /// </summary>
        protected readonly iAppTheme Theme;

        /// <summary>
        /// Services related to the node tree.
        /// </summary>
        protected readonly iTreeNodeService TreeNodeService;

        /// <summary>
        /// Services related to the unit test.
        /// </summary>
        protected readonly iUnitTestService UnitTestService;

        /// <summary>
        /// The identifier for the current class.
        /// </summary>
        protected string ClassName;

        /// <summary>
        /// This is the current class declaration for the action.
        /// </summary>
        protected IClassDeclaration Decl;

        /// <summary>
        /// The identifier for the unit test of the class.
        /// </summary>
        protected string UnitTestName;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ClassActionBase(ICSharpContextActionDataProvider pProvider)
        {
            Provider = pProvider;

            Theme = Locator.Get<iAppTheme>();
            NamingService = Locator.Get<iNamingService>();
            TestProjectService = Locator.Get<iTestProjectService>();
            TreeNodeService = Locator.Get<iTreeNodeService>();
            UnitTestService = Locator.Get<iUnitTestService>();
        }

        /// <summary>
        /// Ask the derived type if this action can be performed on this class.
        /// </summary>
        protected abstract bool isAvailableForClass(IUserDataHolder pCache, IProject pTestProject,
                                                    IClassDeclaration pClass);

        /// <summary>
        /// Checks if a unit test exists for the current class declaration.
        /// </summary>
        public override bool IsAvailable(IUserDataHolder pCache)
        {
            try
            {
                ThrowIf.Null(Provider);
                ITreeNode node = ThrowIf.Null(Provider.SelectedElement);
                Decl = ThrowIf.Null(TreeNodeService.isClassIdentifier(node));
                IProject testProejct = TestProjectService.getProject(Provider.Project);

                ClassName = Decl.NameIdentifier.Name;
                UnitTestName = NamingService.ClassNameToTest(ClassName);

                return isAvailableForClass(pCache, testProejct, Decl);
            }
            catch (IsFalseException)
            {
                return false;
            }
        }
    }
}