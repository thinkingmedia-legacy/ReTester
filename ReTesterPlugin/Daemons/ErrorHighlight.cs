using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using ReTesterPlugin.Daemons;

[assembly: RegisterConfigurableSeverity(ErrorHighlight.ID,
    null,
    "ReTester",
    "Unit test for this class ID was not found.",
    "Some fancy description",
    Severity.ERROR,
    false)]

namespace ReTesterPlugin.Daemons
{
    [ConfigurableSeverityHighlighting(ID, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.ERROR)]
    public class ErrorHighlight : IHighlighting
    {
        /// <summary>
        /// The ID
        /// </summary>
        public const string ID = "ReTester";

        /// <summary>
        /// The tooltip message.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorHighlight([NotNull] string pMessage)
        {
            if (pMessage == null)
            {
                throw new ArgumentNullException("pMessage");
            }
            _message = pMessage;
        }

        /// <summary>
        /// Returns true if data (PSI, text ranges) associated with highlighting is valid
        /// </summary>
        public bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// The message to display.
        /// </summary>
        public string ToolTip
        {
            get { return string.Format("{0} [ReTester]", _message); }
        }

        /// <summary>
        /// Message for this highlighting to show in tooltip and in status bar.
        /// </summary>
        public string ErrorStripeToolTip
        {
            get { return ToolTip; }
        }

        /// <summary>
        /// Specifies the offset from the Range.StartOffset to set the cursor.
        /// </summary>
        public int NavigationOffsetPatch
        {
            get { return 0; }
        }
    }
}