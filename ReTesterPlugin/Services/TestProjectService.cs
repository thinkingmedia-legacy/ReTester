using System;
using System.IO;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Services.Naming;

namespace ReTesterPlugin.Services
{
    /// <summary>
    /// In this class the term "source project" refers to the application project, and "test project" refers to the
    /// project that contains the unit tests for that application.
    /// </summary>
    public static class TestProjectService
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

        /// <summary>
        /// Checks if a unit test exists for a class.
        /// </summary>
        public static bool Exists<TType>([NotNull] TType pClass, iTypeNaming pNaming)
            where TType : class, ITreeNode, ICSharpTypeDeclaration
        {
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }

            try
            {
                IProject project = ThrowIf.Null(pClass.GetProject());
                IProject testProject = ThrowIf.Null(getTestProject(project));

                string nameSpc = pNaming.NameSpace(pClass.OwnerNamespaceDeclaration.DeclaredName);
                string filename = pNaming.Identifier(pClass.NameIdentifier.Name);

                return ProjectService.Exists(testProject, nameSpc, filename);
            }
            catch (IsFalseException)
            {
                return false;
            }
        }
    }
}