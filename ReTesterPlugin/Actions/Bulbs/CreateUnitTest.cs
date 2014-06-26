using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using ReSharperToolKit.Exceptions;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class CreateUnitTest : BulbActionBase
    {
        /// <summary>
        /// Holds the selected context
        /// </summary>
        private readonly ICSharpContextActionDataProvider _provider;

        /// <summary>
        /// The new unit test file in the test project.
        /// </summary>
        private ICSharpFile _unitTestFile;

        /// <summary>
        /// Displays the create message
        /// </summary>
        public override string Text
        {
            get
            {
                ICSharpIdentifier id = _provider.GetSelectedElement<ICSharpIdentifier>(true, true);
                IClassDeclaration decl = _provider.GetSelectedElement<IClassDeclaration>(true, true);

                if (id != null
                    && decl != null
                    && decl.NameIdentifier == id
                    && !UnitTestService.Exists(decl))
                {
                    return string.Format("Create unit test {0}.cs [ReTester]",
                        NamingService.ClassNameToTestName(decl.NameIdentifier.Name));
                }
                return "";
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateUnitTest(ICSharpContextActionDataProvider pProvider)
        {
            _provider = pProvider;
        }

        /// <summary>
        /// Can't add new files to a project during a PSI transaction. So it's done here before the transaction is started.
        /// </summary>
        protected override void ExecuteBeforePsiTransaction(ISolution pSolution,
                                                            IProjectModelTransactionCookie pCookie,
                                                            IProgressIndicator pProgress)
        {
            try
            {
                IClassDeclaration decl = ThrowIf.Null(_provider.GetSelectedElement<IClassDeclaration>(true, true));
                IProject testProejct = ThrowIf.Null(TestProjectService.getTestProject(_provider.Project));
                _unitTestFile = UnitTestService.Create(testProejct, decl);
            }
            catch (IsFalseException)
            {
                _unitTestFile = null;
            }

            base.ExecuteBeforePsiTransaction(pSolution, pCookie, pProgress);
        }

        /// <summary>
        /// Adds the contents for the new unit test file.
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution,
                                                                      IProgressIndicator pProgress)
        {
            IClassDeclaration decl = _provider.GetSelectedElement<IClassDeclaration>(true, true);

            if (_unitTestFile == null
                || decl == null)
            {
                return null;
            }

            return pTextControl=>UnitTestService.Open(decl);
        }
    }
}