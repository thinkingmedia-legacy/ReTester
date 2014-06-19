namespace ReTesterPlugin.Services.Impl
{
    public class StandardNamingPattern : iNamingService
    {
        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        public string ClassNameToTest(string pClassName)
        {
            return string.Format("{0}Test", pClassName);
        }

        /// <summary>
        /// Converts a project name into the test project.
        /// </summary>
        public string ProjectToTestProject(string pProject)
        {
            return string.Format("{0}Tests", pProject);
        }
    }
}