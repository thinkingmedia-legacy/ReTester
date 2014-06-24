using System;
using JetBrains.Annotations;

namespace ReTesterPlugin.Services
{
    public static class AppTheme
    {
        /// <summary>
        /// Adds the plugin name to end.
        /// </summary>
        public static string ActionText([NotNull] string pText)
        {
            if (pText == null)
            {
                throw new ArgumentNullException("pText");
            }
            return string.Format("{0} [ReTester]", pText);
        }
    }
}