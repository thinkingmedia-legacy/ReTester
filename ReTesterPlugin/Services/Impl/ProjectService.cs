using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.Util;

namespace ReTesterPlugin.Services.Impl
{
    /// <summary>
    /// General services related to a project.
    /// </summary>
    public class ProjectService : iProjectService
    {
        /// <summary>
        /// Finds or creates a folder using a namespace.
        /// </summary>
        public IProjectFolder getFolder(IProject pProject, string pNameSpace)
        {
            FileSystemPath path =
                FileSystemPath.Parse(pProject.ProjectFileLocation.Directory.FullPath + @"\" +
                                     pNameSpace.Replace(".", @"\"));
            return pProject.GetOrCreateProjectFolder(path);
        }
    }
}