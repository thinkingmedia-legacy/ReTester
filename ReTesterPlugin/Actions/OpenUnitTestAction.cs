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
    [ContextAction(Name = "OpenUnitTest", Description = "Opens an existing unit test for a class", Group = "C#")]
    public class OpenUnitTestAction : ClassActionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenUnitTestAction(ICSharpContextActionDataProvider pProvider) 
            : base(pProvider)
        {
        }

        /// <summary>
        /// Displays the recommended filename.
        /// </summary>
        public override string Text
        {
            get { return Theme.ActionText(string.Format("Open unit test {0}.cs", UnitTestName ?? "")); }
        }

        /// <summary>
        /// Creates the new unit test file.
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            if (Decl != null)
            {
                UnitTestService.Open(Decl);
            }
            return null;
        }

        /// <summary>
        /// Can a unit test be created for this class?
        /// </summary>
        protected override bool isAvailableForClass(IUserDataHolder pCache, IProject pTestProject,
                                                    IClassDeclaration pClass)
        {
            return pTestProject != null && UnitTestService.Exists(Decl);
        }
    }
}