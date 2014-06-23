namespace ReTesterPlugin.Modules.Services
{
    public interface iNamingService
    {
        /// <summary>
        /// Converts a namespace for a class into the namespace for the unit test.
        /// </summary>
        string NameSpaceToTestNameSpace(string pNameSpace);

        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        string ClassNameToTest(string pClassName);

        /// <summary>
        /// Converts a project name into the test project.
        /// </summary>
        string ProjectToTestProject(string pProject);
    }
}