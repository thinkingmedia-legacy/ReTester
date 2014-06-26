using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;

namespace ReTesterPlugin.Services
{
    public static class TestProjectService
    {
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
        /// Finds the source project from a test project.
        /// </summary>
        public static IProject getSourceProject([NotNull] IProject pProject)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }

            if (!isTestProject(pProject))
            {
                return getTestProject(pProject) != null ? Validate(pProject) : null;
            }

            string sourceName = NamingService.TestProjectToProject(pProject.Name);
            IProject sourceProject = pProject.GetSolution().GetProjectByName(sourceName);
            return Validate(sourceProject);
        }

        /// <summary>
        /// Finds the test project from a source project.
        /// </summary>
        public static IProject getTestProject([NotNull] IProject pSource)
        {
            if (pSource == null)
            {
                throw new ArgumentNullException("pSource");
            }

            string projectName = NamingService.ProjectToTestProject(pSource.Name);
            IProject project = pSource.GetSolution().GetProjectByName(projectName);
            return project != null && isValid(project) ? project : null;
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