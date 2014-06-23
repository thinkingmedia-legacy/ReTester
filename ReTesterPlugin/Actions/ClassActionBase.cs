using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReTesterPlugin.Editors;
using ReTesterPlugin.Exceptions;
using ReTesterPlugin.Modules;
using ReTesterPlugin.Modules.Factories;
using ReTesterPlugin.Modules.Services;

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
            ElementEditorFactory = Locator.Get<iElementEditorFactory>();
        }

        /// <summary>
        /// Used to create specialized editors
        /// </summary>
        protected iElementEditorFactory ElementEditorFactory;

        /// <summary>
        /// Used to edit the current class.
        /// </summary>
        protected iClassEditor ClassEditor;

        /// <summary>
        /// Used to edit the C source file.
        /// </summary>
        protected iSourceEditor SourceEditor;

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
                IProject testProejct = TestProjectService.getProject(Provider.Project);

                Decl = ThrowIf.Null(TreeNodeService.isClassIdentifier(node));
                ClassName = Decl.NameIdentifier.Name;
                UnitTestName = NamingService.ClassNameToTest(ClassName);

                CSharpElementFactory factory = CSharpElementFactory.GetInstance(Provider.PsiModule);
                ClassEditor = ElementEditorFactory.CreateClassEditor(factory, Decl);
                SourceEditor = ElementEditorFactory.CreateSourceEditor(factory, Provider.PsiFile);

                ThrowIf.False(isAvailableForClass(pCache, testProejct, Decl));
                return true;
            }
            catch (IsFalseException)
            {
                Clear();
                return false;
            }
        }

        /// <summary>
        /// Clear the state
        /// </summary>
        private void Clear()
        {
            Decl = null;
            ClassName = null;
            UnitTestName = null;
            ClassEditor = null;
        }
    }
}