using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using ReSharperToolKit.Modules.Services;

namespace ReTesterPlugin.Modules.Impl
{
    public class TestProjectService : iTestProjectService
    {
        /// <summary>
        /// Used to compute the name of the test project.
        /// </summary>
        private readonly iNamingService _naming;

        /// <summary>
        /// Constructor
        /// </summary>
        public TestProjectService([NotNull] iNamingService pNaming)
        {
            if (pNaming == null)
            {
                throw new ArgumentNullException("pNaming");
            }
            _naming = pNaming;
        }

        /// <summary>
        /// Finds and verifies the target test project.
        /// </summary>
        /// <param name="pSource">The project that is being tested.</param>
        /// <returns>Null if project doesn't exist or is invalid.</returns>
        public IProject getProject([NotNull] IProject pSource)
        {
            if (pSource == null)
            {
                throw new ArgumentNullException("pSource");
            }
            string projectName = _naming.ProjectToTestProject(pSource.Name);
            IProject project = pSource.GetSolution().GetProjectByName(projectName);
            if (project != null)
            {
                return project.IsOpened && project.IsValid() ? project : null;
            }
            return null;
        }
    }
}