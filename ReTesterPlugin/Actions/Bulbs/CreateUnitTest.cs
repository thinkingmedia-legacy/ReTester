using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Services.Templates;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class CreateUnitTest : BaseAction<IClassDeclaration>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateUnitTest(ICSharpContextActionDataProvider pProvider)
            : base(pProvider, ePROJECT_SCOPE.SOURCE)
        {
        }

        /// <summary>
        /// Just opens the new file.
        /// </summary>
        protected override Action<ITextControl> Action(ISolution pSolution,
                                                       IProgressIndicator pProgress,
                                                       IProject pProject,
                                                       IClassDeclaration pType,
                                                       ICSharpIdentifier pId)
        {
            return pTextControl=>FilesService.Open(pType, NamingService.TestNaming);
        }

        /// <summary>
        /// Creates the new unit test file outside of the PSI transaction.
        /// </summary>
        protected override void BeforeAction(IProject pProject,
                                             IClassDeclaration pType,
                                             ICSharpIdentifier pId)
        {
            TemplateService.Create(pProject, pType, NamingService.TestNaming, TemplateService.UnitTest);
        }

        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected override string getText(IProject pProject, IClassDeclaration pType, ICSharpIdentifier pId)
        {
            return string.Format("Create unit test {0}.cs [ReTester]", NamingService.TestNaming.Identifier(pId.Name));
        }

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected override bool isEnabled(IProject pProject, IClassDeclaration pType, ICSharpIdentifier pId)
        {
            return !FilesService.Exists(pType, NamingService.TestNaming);
        }
    }
}