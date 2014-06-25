using System;
using System.Collections.Generic;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;

namespace ReTesterPlugin.Daemons.TestExists
{
    public class TestMissingDaemonProcess : IDaemonStageProcess
    {
        /// <summary>
        /// The current process
        /// </summary>
        private readonly IDaemonProcess _process;

        /// <summary>
        /// Constructor
        /// </summary>
        public TestMissingDaemonProcess(IDaemonProcess pProcess)
        {
            _process = pProcess;
        }

        /// <summary>
        /// Executes the process.
        /// </summary>
        public void Execute(Action<DaemonStageResult> pCommitter)
        {
            if (!_process.FullRehighlightingRequired)
            {
                return;
            }

            List<HighlightingInfo> highlights = new List<HighlightingInfo>();
            IPsiSourceFile sourceFile = _process.SourceFile;
            IPsiServices psiServices = sourceFile.GetPsiServices();
            IFile file = psiServices.Files.GetDominantPsiFile<CSharpLanguage>(sourceFile);
            if (file == null)
            {
                return;
            }

            file.ProcessChildren<IClassDeclaration>(pDecl=>
                                                    {
                                                        TestMissingDaemonHighlighting highlighting = new TestMissingDaemonHighlighting(pDecl);
                                                        DocumentRange range = pDecl.NameIdentifier.GetDocumentRange();
                                                        HighlightingInfo info = new HighlightingInfo(range, highlighting);
                                                        highlights.Add(info);
                                                    });

            pCommitter(new DaemonStageResult(highlights));
        }

        /// <summary>
        /// Whole daemon process
        /// </summary>
        public IDaemonProcess DaemonProcess { get { return _process; }}
    }
}