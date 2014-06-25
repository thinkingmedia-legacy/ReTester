using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperToolKit.Daemons;

namespace ReTesterPlugin.Daemons
{
    public class TestMissingProcess : GenericDaemonProcessor<IClassDeclaration, CSharpLanguage, ICSharpFile>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TestMissingProcess(IDaemonProcess pProcess)
            : base(pProcess)
        {
        }

        /// <summary>
        /// Highlight the class name.
        /// </summary>
        protected override IEnumerable<HighlightingInfo> getHighlights(ICSharpFile pFile, IClassDeclaration pNode)
        {
            ErrorHighlight highlighting = new ErrorHighlight("Unit test for this class could not be found.");
            return new[] {new HighlightingInfo(pNode.NameIdentifier.GetDocumentRange(), highlighting)};
        }
    }
}