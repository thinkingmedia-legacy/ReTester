namespace ReTesterPlugin.Modules.Impl
{
    public class AppTheme : iAppTheme
    {
        /// <summary>
        /// Adds the plugin name to end.
        /// </summary>
        public string ActionText(string pText)
        {
            return string.Format("{0} [ReTester]", pText);
        }
    }
}