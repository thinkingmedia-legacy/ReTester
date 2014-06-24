using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.TextControl;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
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
                IClassDeclaration decl = _provider.GetSelectedElement<IClassDeclaration>(true, true);
                if (decl != null && !UnitTestService.Exists(decl))
                {
                    return string.Format("Create unit test {0}.cs",
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
                IProject testProejct = ThrowIf.Null(TestProjectService.getProject(_provider.Project));

                string nameSpc = NamingService.NameSpaceToTestNameSpace(decl.OwnerNamespaceDeclaration.DeclaredName);
                string unitTest = NamingService.ClassNameToTestName(decl.NameIdentifier.Name);

                IProjectFile projectFile =
                    ThrowIf.Null(ProjectService.getFileOrCreate(testProejct, nameSpc, unitTest + ".cs"));
                IPsiSourceFile sourceFile = ThrowIf.Null(projectFile.ToSourceFile());
                _unitTestFile = ThrowIf.Null(sourceFile.GetPrimaryPsiFile() as ICSharpFile);
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
            if (_unitTestFile == null)
            {
                return null;
            }

            try
            {
                IClassDeclaration decl = ThrowIf.Null(_provider.GetSelectedElement<IClassDeclaration>(true, true));

                string nameSpc = NamingService.NameSpaceToTestNameSpace(decl.OwnerNamespaceDeclaration.DeclaredName);
                string unitTest = NamingService.ClassNameToTestName(decl.NameIdentifier.Name);

                CSharpElementFactory factory = CSharpElementFactory.GetInstance(_provider.PsiModule);
                ICSharpFile tmpFile = factory.CreateFile(string.Format("public class {0} {{}}", unitTest));
                _unitTestFile.AddTypeDeclarationAfter(tmpFile.TypeDeclarations[0], null);
            }
            catch (IsFalseException)
            {
                _unitTestFile = null;
            }

            return null;
        }
    }
}