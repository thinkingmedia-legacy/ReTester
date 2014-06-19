namespace ReTesterPlugin.Services
{
    public interface iNamingService
    {
        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        string ClassNameToTest(string pClassName);

        /// <summary>
        /// Adds the plugin name to end.
        /// </summary>
        string ActionText(string pText);
    }
}