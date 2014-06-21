using JetBrains.ProjectModel;

namespace ReTesterPlugin.Services
{
    /// <summary>
    /// General services related to a project.
    /// </summary>
    public interface iProjectService
    {
        /// <summary>
        /// Finds or creates a folder using a namespace.
        /// </summary>
        IProjectFolder getFolder(IProject pProject, string pNameSpace);
    }
}