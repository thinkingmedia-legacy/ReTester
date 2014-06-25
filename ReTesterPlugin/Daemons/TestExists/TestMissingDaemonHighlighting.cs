using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Daemons.TestExists;
using ReTesterPlugin.Services;

[assembly: RegisterConfigurableSeverity(TestMissingDaemonHighlighting.SEVERITY_ID,
    null,
    "ReTester",
    "Unit test for this class ID was not found.",
    "Some fancy description",
    Severity.ERROR,
    false)]

namespace ReTesterPlugin.Daemons.TestExists
{
    [ConfigurableSeverityHighlighting(SEVERITY_ID, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.ERROR)]
    public class TestMissingDaemonHighlighting : IHighlighting
    {
        /// <summary>
        /// The ID
        /// </summary>
        public const string SEVERITY_ID = "TestExists";

        /// <summary>
        /// The class to highlight
        /// </summary>
        private readonly IClassDeclaration _decl;

        /// <summary>
        /// Constructor
        /// </summary>
        public TestMissingDaemonHighlighting(IClassDeclaration pDecl)
        {
            _decl = pDecl;
        }

        /// <summary>
        /// Returns true if data (PSI, text ranges) associated with highlighting is valid
        /// </summary>
        public bool IsValid()
        {
            return _decl != null;
        }

        /// <summary>
        /// The message to display.
        /// </summary>
        public string ToolTip { get { return "Unit test for this class ID was not found. [ReTester]";  } }

        /// <summary>
        /// Message for this highlighting to show in tooltip and in status bar.
        /// </summary>
        public string ErrorStripeToolTip { get { return ToolTip; } }

        /// <summary>
        /// Specifies the offset from the Range.StartOffset to set the cursor.
        /// </summary>
        public int NavigationOffsetPatch { get { return 0; } }
    }
}