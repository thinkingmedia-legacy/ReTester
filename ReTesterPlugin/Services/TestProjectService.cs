using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;

namespace ReTesterPlugin.Services
{
    public static class TestProjectService
    {
        /// <summary>
        /// Finds and verifies the target test project.
        /// </summary>
        /// <param name="pSource">The project that is being tested.</param>
        /// <returns>Null if project doesn't exist or is invalid.</returns>
        public static IProject getProject([NotNull] IProject pSource)
        {
            if (pSource == null)
            {
                throw new ArgumentNullException("pSource");
            }

            string projectName = NamingService.ProjectToTestProject(pSource.Name);
            IProject project = pSource.GetSolution().GetProjectByName(projectName);
            if (project != null)
            {
                return project.IsOpened && project.IsValid() ? project : null;
            }

            return null;
        }
    }
}