using JetBrains.ActionManagement;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Services.Templates;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateMockInterfaces")]
    public class CreateMockInterfaces : ProjectFilesBase<ICSharpFile, IInterfaceDeclaration>
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
                                        IInterfaceDeclaration pType)
        {
            if (FilterService.isPublic(pType.ModifiersList))
            {
                TemplateService.Create(pTestProject, pType, NamingService.MockInterfaces, TemplateService.MockInterface);
            }
        }
    }
}