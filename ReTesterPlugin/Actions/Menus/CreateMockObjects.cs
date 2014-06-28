using JetBrains.ActionManagement;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Services.Templates;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateMockObjects")]
    public class CreateMockObjects : ProjectFilesBase<ICSharpFile, IClassDeclaration>
    {
        /// <summary>
        /// Which projects should the action process the files for?
        /// </summary>
        protected override IProject FilesFrom(IProject pSourceProject, IProject pTestProject)
        {
            return pSourceProject;
        }

        /// <summary>
        /// Process a file from the project.
        /// </summary>
        protected override void Process(IProject pTestProject,
                                        IProject pSourceProject,
                                        ICSharpFile pFile,
                                        IClassDeclaration pType)
        {
            if (FilterService.isAbstract(pType.ModifiersList))
            {
                TemplateService.Create(pTestProject, pType, NamingService.MockObjects, TemplateService.MockObject);
            }
        }
    }
}