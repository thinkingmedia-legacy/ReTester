using JetBrains.ProjectModel;

namespace ReTesterPlugin.Modules
{
    /// <summary>
    /// Handles most tasks associated with the target test project.
    /// </summary>
    public interface iTestProjectService
    {
        /// <summary>
        /// Finds and verifies the target test project.
        /// </summary>
        /// <param name="pSource">The project that is being tested.</param>
        /// <returns>Null if project doesn't exist or is invalid.</returns>
        IProject getProject(IProject pSource);
    }
}