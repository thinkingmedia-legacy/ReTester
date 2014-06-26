using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.Util;
using ReSharperToolKit.Exceptions;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateUnitTests")]
    public class CreateUnitTests : IActionHandler
    {
        /// <summary>
        /// If any element is not null, then that resource can have unit test.
        /// </summary>
        public bool Update(IDataContext pContext, ActionPresentation pPresentation, DelegateUpdate pNextUpdate)
        {
            IProject project = pContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.Project);
            if (project != null)
            {
                if (TestProjectService.isTestProject(project) 
                    || TestProjectService.getTestProject(project) != null)
                {
                    return true;
                }
            }

            IProjectModelElement element = pContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.PROJECT_MODEL_ELEMENT);
            return (element != null);
        }

        public void Execute(IDataContext pContext, DelegateExecute pNextExecute)
        {
            IProject sourceProject, testProject;

            try
            {
                IProject project = ThrowIf.Null(pContext.GetData(JetBrains.ProjectModel.DataContext.DataConstants.Project));
                sourceProject = ThrowIf.Null(TestProjectService.getSourceProject(project));
                testProject = ThrowIf.Null(TestProjectService.getTestProject(project));
            }
            catch (IsFalseException)
            {
                return;
            }

            foreach (IProjectFile file in sourceProject.GetAllProjectFiles())
            {
                try
                {
                    IPsiSourceFile sourceFile = ThrowIf.Null(file.ToSourceFile());
                    ICSharpFile cSharpFile = ThrowIf.Null(sourceFile.GetPrimaryPsiFile() as ICSharpFile);
                    UnitTestService.Create(testProject, cSharpFile);
                }
                catch (IsFalseException)
                {
                }
            }
        }
    }
}
