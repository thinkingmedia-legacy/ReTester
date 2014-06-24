using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReTesterPlugin.Actions
{
    /// <summary>
    /// Enables an action of a class to create a matching unit test.
    /// </summary>
    [ContextAction(Name = "CreateUnitTest", Description = "Creates a unit test for a class", Group = "C#")]
    public class CreateUnitTestAction : ReTesterAction
    {
        private ICSharpFile _file;

        /// <summary>
        /// Displays the recommended filename.
        /// </summary>
        public override string Text
        {
            get
            {
                return
                    Theme.ActionText(string.Format("Create unit test {0}.cs",
                        SelectedClass == null ? "" : SelectedClass.UnitTestName));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateUnitTestAction(ICSharpContextActionDataProvider pProvider)
            : base(pProvider)
        {
        }

        /// <summary>
        /// Executes QuickFix or ContextAction. Returns post-execute method.
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            if (SelectedClass == null)
            {
                return null;
            }

            UnitTestService.Create(_file, SelectedClass.Decl, Provider.PsiModule);

            SelectedClass.SourceEditor.AddUsing("ReTester.Attributes.UnitTestAttribute");
            SelectedClass.ClassEditor.AddAttribute(string.Format("UnitTest(\"{0}\")", Guid.NewGuid()));

            UnitTestService.Open(SelectedClass.Decl);
            return null;
        }

        /// <summary>
        /// Can a unit test be created for this class?
        /// </summary>
        protected override bool isAvailableForClass(IUserDataHolder pCache)
        {
            IProject testProject = TestProjectService.getProject(Provider.Project);
            return testProject != null && !UnitTestService.Exists(SelectedClass.Decl);
        }

        /// <summary>
        /// Must add new files outside of a PSI transaction.
        /// </summary>
        public override void Execute(ISolution pSolution, ITextControl pTextControl)
        {
            if (SelectedClass != null)
            {
                _file = UnitTestService.PreCreate(SelectedClass.Decl, Provider.PsiModule);
            }
            base.Execute(pSolution, pTextControl);
        }
    }
}