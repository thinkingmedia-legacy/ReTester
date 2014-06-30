using System;
using System.IO;
using JetBrains.Annotations;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Features.Naming;

namespace ReTesterPlugin.Services
{
    /// <summary>
    /// In this class the term "source project" refers to the application project, and "test project" refers to the
    /// project that contains the unit tests for that application.
    /// </summary>
    public static class FilesService
    {
        /// <summary>
        /// Returns the project only if it can be processed.
        /// </summary>
        private static IProject Validate(IProject pProject)
        {
            if (pProject == null)
            {
                return null;
            }
            return isValid(pProject)
                ? pProject
                : null;
        }

        /// <summary>
        /// Calculates the full path to the unit test file from a class identifier.
        /// </summary>
        private static string getFile<TType>([NotNull] IProject pProject,
                                             [NotNull] TType pType,
                                             [NotNull] iTypeNaming pNaming)
            where TType : class, ITreeNode, ICSharpTypeDeclaration
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }
            if (pNaming == null)
            {
                throw new ArgumentNullException("pNaming");
            }

            string nameSpc = pNaming.NameSpace(pType.OwnerNamespaceDeclaration.DeclaredName);
            string unitTest = pNaming.Identifier(pType.NameIdentifier.Name);

            return ProjectService.getFileName(pProject, nameSpc, unitTest);
        }

        /// <summary>
        /// Checks if a project is usable.
        /// </summary>
        private static bool isValid([NotNull] IProject pProject)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            return pProject.IsOpened && pProject.IsValid();
        }

        /// <summary>
        /// Checks if a unit test exists for a class.
        /// </summary>
        public static bool Exists<TType>([NotNull] TType pType, [NotNull] iTypeNaming pNaming)
            where TType : class, ITreeNode, ICSharpTypeDeclaration
        {
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }
            if (pNaming == null)
            {
                throw new ArgumentNullException("pNaming");
            }

            try
            {
                IProject project = ThrowIf.Null(pType.GetProject());
                IProject testProject = ThrowIf.Null(getTestProject(project));

                string nameSpc = pNaming.NameSpace(pType.OwnerNamespaceDeclaration.DeclaredName);
                string filename = pNaming.Identifier(pType.NameIdentifier.Name);

                return ProjectService.Exists(testProject, nameSpc, filename);
            }
            catch (IsFalseException)
            {
                return false;
            }
        }

        /// <summary>
        /// Opens the unit test for a class.
        /// </summary>
        public static bool Open<TType>([NotNull] TType pType, iTypeNaming pNaming)
            where TType : class, ITreeNode, ICSharpTypeDeclaration
        {
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }

            try
            {
                IProject project = ThrowIf.Null(pType.GetProject());
                IProject testProject = ThrowIf.Null(getTestProject(project));

                string outFile = getFile(testProject, pType, pNaming);
                ThrowIf.False(File.Exists(outFile));

                ISolution solution = ThrowIf.Null(project.GetSolution());
                EditorManager editor = EditorManager.GetInstance(solution);

                editor.OpenFile(FileSystemPath.Parse(outFile), true, TabOptions.Default);
            }
            catch (IsFalseException)
            {
                return false;
            }
            catch (InvalidPathException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Using the project argument as a guide. This method will attempt to find the project
        /// that would be the source project.
        /// </summary>
        public static IProject getSourceProject([NotNull] IProject pUnknownProject)
        {
            if (pUnknownProject == null)
            {
                throw new ArgumentNullException("pUnknownProject");
            }

            IProject sourceProject = isSourceProject(pUnknownProject) ? pUnknownProject : null;

            if (sourceProject == null)
            {
                string sourceName = NamingService.TestProjectToProject(pUnknownProject.Name);
                sourceProject = pUnknownProject.GetSolution().GetProjectByName(sourceName);
            }

            return Validate(sourceProject);
        }

        /// <summary>
        /// Using the project argument as a guide. This method will attempt to find the project
        /// that would be the test project.
        /// </summary>
        public static IProject getTestProject([NotNull] IProject pUnknownProject)
        {
            if (pUnknownProject == null)
            {
                throw new ArgumentNullException("pUnknownProject");
            }

            IProject testProject = isTestProject(pUnknownProject) ? pUnknownProject : null;

            if (testProject == null)
            {
                string projectName = NamingService.ProjectToTestProject(pUnknownProject.Name);
                testProject = pUnknownProject.GetSolution().GetProjectByName(projectName);
            }

            return Validate(testProject);
        }

        /// <summary>
        /// True if the project is a source project.
        /// </summary>
        public static bool isSourceProject(IProject pProject)
        {
            return !isTestProject(pProject);
        }

        /// <summary>
        /// True if the project is a test project.
        /// </summary>
        public static bool isTestProject([NotNull] IProject pProject)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }

            return NamingService.isTestProjectName(pProject.Name);
        }
    }
}