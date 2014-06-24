using System;
using JetBrains.Annotations;

namespace ReTesterPlugin.Modules.Impl
{
    public class AppTheme : iAppTheme
    {
        /// <summary>
        /// Adds the plugin name to end.
        /// </summary>
        public string ActionText([NotNull] string pText)
        {
            if (pText == null)
            {
                throw new ArgumentNullException("pText");
            }
            return string.Format("{0} [ReTester]", pText);
        }
    }
}