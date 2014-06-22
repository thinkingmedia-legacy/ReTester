using System;
using JetBrains.Application;
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
    public class CreateUnitTestAction : ClassActionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateUnitTestAction(ICSharpContextActionDataProvider pProvider) 
            : base(pProvider)
        {
        }

        /// <summary>
        /// Displays the recommended filename.
        /// </summary>
        public override string Text
        {
            get { return Theme.ActionText(string.Format("Create unit test {0}.cs", UnitTestName ?? "")); }
        }

        /// <summary>
        /// Creates the new unit test file.
        /// </summary>
        public override void Execute(ISolution pSolution, ITextControl pTextControl)
        {
            base.Execute(pSolution, pTextControl);
        }

        protected override Action<ITextControl> ExecuteAfterPsiTransaction(ISolution pSolution,
                                                                           IProjectModelTransactionCookie pCookie,
                                                                           IProgressIndicator pProgress)
        {
            if (Decl != null)
            {
                UnitTestService.Create(Decl, Provider.PsiModule);
                UnitTestService.Open(Decl);
            }
            return null;
        }


        /// <summary>
        /// Executes QuickFix or ContextAction. Returns post-execute method.
        /// </summary>
        /// <returns>
        /// Action to execute after document and PSI transaction finish. Use to open TextControls, navigate caret, etc.
        /// </returns>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            return null;
        }

        /// <summary>
        /// Can a unit test be created for this class?
        /// </summary>
        protected override bool isAvailableForClass(IUserDataHolder pCache, IProject pTestProject,
                                                    IClassDeclaration pClass)
        {
            return pTestProject != null && !UnitTestService.Exists(Decl);
        }
    }
}