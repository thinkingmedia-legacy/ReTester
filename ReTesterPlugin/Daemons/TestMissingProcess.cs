using System.Collections.Generic;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Daemons;

namespace ReTesterPlugin.Daemons
{
    public class TestMissingProcess : GenericDaemonProcessor<IClassDeclaration, CSharpLanguage, ICSharpFile>
    {
        public TestMissingProcess(IDaemonProcess pProcess)
            : base(pProcess)
        {
        }

        /// <summary>
        /// Highlight the class name.
        /// </summary>
        protected override IEnumerable<HighlightingInfo> getHighlights(ICSharpFile pFile, IClassDeclaration pNode)
        {
/*
            IAttribute id = ClassService.getAttributes<ReTesterIdAttribute>(pNode).FirstOrDefault();
            if (id == null)
            {
                return null;
            }
*/
            return null;

/*
            ErrorHighlight highlighting = new ErrorHighlight("Unit test for this class could not be found.");
            return new[] {new HighlightingInfo(id.Name.GetDocumentRange(), highlighting)};
*/
        }
    }
}