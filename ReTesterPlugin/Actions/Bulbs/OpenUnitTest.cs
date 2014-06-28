using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class OpenUnitTest : BulbActionBase
    {
        private readonly ICSharpContextActionDataProvider _provider;

        /// <summary>
        /// Displays the open message.
        /// </summary>
        public override string Text
        {
            get
            {
                ICSharpIdentifier id = _provider.GetSelectedElement<ICSharpIdentifier>(true, true);
                IClassDeclaration decl = _provider.GetSelectedElement<IClassDeclaration>(true, true);

                if (id != null &&
                    decl != null && 
                    decl.NameIdentifier == id &&
                    TestProjectService.Exists(decl,NamingService.TestNaming))
                {
                    return string.Format("Open unit test {0}.cs [ReTester]",
                        NamingService.NameToTestName(decl.NameIdentifier.Name));
                }
                return "";
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenUnitTest(ICSharpContextActionDataProvider pProvider)
        {
            _provider = pProvider;
        }

        /// <summary>
        /// Creates the new unit test file.
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            IClassDeclaration decl = _provider.GetSelectedElement<IClassDeclaration>(true, true);
            if (decl != null)
            {
                UnitTestService.Open(decl);
            }
            return null;
        }
    }
}