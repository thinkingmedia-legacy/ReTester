using System.Collections.Generic;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Features;
using ReTesterPlugin.Services;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace ReTesterPlugin.Actions.Menus
{
    /// <summary>
    /// Handles the work of creating a new file using a template and naming convention.
    /// </summary>
    /// <typeparam name="TFileType">Usually ICSharpFile</typeparam>
    /// <typeparam name="TDeclarationType">Can be IClassDeclaration or IInterfaceDeclaration.</typeparam>
    public abstract class CreateFile<TFileType, TDeclarationType> : IActionHandler
        where TFileType : class, IFile
        where TDeclarationType : class, ITreeNode, ICSharpTypeDeclaration
    {
        private readonly iFeatureType _featureType;

        /// <summary>
        /// Constructor
        /// </summary>
        protected CreateFile(iFeatureType pFeatureType)
        {
            _featureType = pFeatureType;
        }

        /// <summary>
        /// If any element is not null, then that resource can have unit test.
        /// </summary>
        public bool Update(IDataContext pContext, ActionPresentation pPresentation, DelegateUpdate pNextUpdate)
        {
            try
            {
                IProject project = ThrowIf.Null(pContext.GetData(DataConstants.Project));

                ThrowIf.Null(FilesService.getSourceProject(project));
                ThrowIf.Null(FilesService.getTestProject(project));
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
                sourceProject = ThrowIf.Null(FilesService.getSourceProject(unknown));
                testProject = ThrowIf.Null(FilesService.getTestProject(unknown));
            }
            catch (IsFalseException)
            {
                return;
            }

            foreach (TFileType file in ProjectService.getSourceFiles<TFileType>(sourceProject))
            {
                List<TDeclarationType> types = SourceFileService.getAllNodesOf<TDeclarationType>(file);
                types.ForEach(pType=>
                              {
                                  if (_featureType.Filter.isMatch(pType.ModifiersList))
                                  {
                                      TemplateService.Create(testProject, pType, _featureType);
                                  }
                              });
            }
        }
    }
}