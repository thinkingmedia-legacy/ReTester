namespace ReTesterPlugin.Services
{
    public class NamingService : iNamingService
    {
        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        public string ClassNameToTest(string pClassName)
        {
            return string.Format("{0}Tests", pClassName);
        }

        /// <summary>
        /// Adds the plugin name to end.
        /// </summary>
        public string ActionText(string pText)
        {
            return string.Format("{0} [ReTester]", pText);
        }
    }
}