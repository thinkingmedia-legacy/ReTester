using System.Collections.Generic;
using System.Linq;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;

namespace ReTesterPlugin.Daemons.TestExists
{
    [DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
    public class TestMissingDaemonStage : IDaemonStage
    {
        /// <summary>
        /// Creates a code analysis process corresponding to this stage for analyzing a file.
        /// </summary>
        /// <returns>
        /// Code analysis process to be executed or <c>null</c> if this stage is not available for this file.
        /// </returns>
        public IEnumerable<IDaemonStageProcess> CreateProcess(IDaemonProcess pProcess,
                                                              IContextBoundSettingsStore pSettings,
                                                              DaemonProcessKind pProcessKind)
        {
            IPsiSourceFile sourceFile = pProcess.SourceFile;
            IPsiServices psiServices = sourceFile.GetPsiServices();
            IFile file = psiServices.Files.GetDominantPsiFile<CSharpLanguage>(sourceFile);
            if (file == null)
            {
                return Enumerable.Empty<IDaemonStageProcess>();
            }

            return new[] {new TestMissingDaemonProcess(pProcess)};
        }

        /// <summary>
        /// Check the error stripe indicator necessity for this stage after processing given <paramref name="pSourceFile"/>
        /// </summary>
        public ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile pSourceFile, IContextBoundSettingsStore pSettingsStore)
        {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }
    }
}