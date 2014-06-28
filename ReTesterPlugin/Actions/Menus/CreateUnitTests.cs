using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Services;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Services.Templates;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateUnitTests")]
    public class CreateUnitTests : ProjectFilesBase<ICSharpFile,IClassDeclaration>
    {
        /// <summary>
        /// Process a file from the project.
        /// </summary>
        protected override void Process(IProject pTestProject, IProject pSourceProject, ICSharpFile pFile, IClassDeclaration pType)
        {
            if (ClassService.isSafelyPublic(pType.ModifiersList))
            {
                TemplateService.Create(pTestProject, pType, NamingService.TestNaming, TemplateService.UnitTest);
            }
        }

        /// <summary>
        /// Which projects should the action process the files for?
        /// </summary>
        protected override IProject FilesFrom(IProject pSourceProject, IProject pTestProject)
        {
            return pSourceProject;
        }
    }
}