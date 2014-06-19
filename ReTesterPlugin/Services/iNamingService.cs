namespace ReTesterPlugin.Services
{
    public interface iNamingService
    {
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