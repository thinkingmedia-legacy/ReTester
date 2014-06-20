using JetBrains.ProjectModel;
using ReTesterPlugin.Exceptions;

namespace ReTesterPlugin.Services.Impl
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
        public TestProjectService(iNamingService pNaming)
        {
            _naming = pNaming;
        }

        /// <summary>
        /// Finds and verifies the target test project.
        /// </summary>
        /// <param name="pSource">The project that is being tested.</param>
        /// <returns>Null if project doesn't exist or is invalid.</returns>
        public IProject getProject(IProject pSource)
        {
            string projectName = _naming.ProjectToTestProject(pSource.Name);
            IProject project = pSource.GetSolution().GetProjectByName(projectName);
            if (project != null)
            {
                return project.IsOpened ? project : null;
            }
            return null;
        }
    }
}