using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Services;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace ReTesterPlugin.Actions.Menus
{
    public abstract class ProjectFilesBase<TFileType, TDeclarationType> : IActionHandler
        where TFileType : class, IFile
        where TDeclarationType : class, ITreeNode
    {
        /// <summary>
        /// Process a file from the project.
        /// </summary>
        protected abstract void Process(IProject pTestProject, IProject pSourceProject, TFileType pFile, TDeclarationType pType);

        /// <summary>
        /// Which projects should the action process the files for?
        /// </summary>
        protected abstract IProject FilesFrom(IProject pSourceProject, IProject pTestProject);

        /// <summary>
        /// If any element is not null, then that resource can have unit test.
        /// </summary>
        public bool Update(IDataContext pContext, ActionPresentation pPresentation, DelegateUpdate pNextUpdate)
        {
            try
            {
                IProject project = ThrowIf.Null(pContext.GetData(DataConstants.Project));

                ThrowIf.Null(TestProjectService.getSourceProject(project));
                ThrowIf.Null(TestProjectService.getTestProject(project));
            }
            catch (IsFalseException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates the unit tests.
        /// </summary>
        public void Execute(IDataContext pContext, DelegateExecute pNextExecute)
        {
            IProject sourceProject, testProject;

            try
            {
                IProject unknown =
                    ThrowIf.Null(pContext.GetData(DataConstants.Project));
                sourceProject = ThrowIf.Null(TestProjectService.getSourceProject(unknown));
                testProject = ThrowIf.Null(TestProjectService.getTestProject(unknown));
            }
            catch (IsFalseException)
            {
                return;
            }

            IProject project = FilesFrom(sourceProject, testProject);
            if (project == null)
            {
                return;
            }

            foreach (TFileType file in ProjectService.getSourceFiles<TFileType>(project))
            {
                List<TDeclarationType> types = SourceFileService.getAllNodesOf<TDeclarationType>(file);
                types.ForEach(pType => Process(testProject, sourceProject, file, pType));
            }
        }
    }
}