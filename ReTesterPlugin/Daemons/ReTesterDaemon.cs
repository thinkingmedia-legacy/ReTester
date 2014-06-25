using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Daemons;

namespace ReTesterPlugin.Daemons
{
    [DaemonStage(StagesBefore = new[] {typeof (LanguageSpecificDaemonStage)})]
    public class ReTesterDaemon : GenericDaemonStage<CSharpLanguage, ICSharpFile>
    {
        /// <summary>
        /// Called to create processors only when a C sharp file is being processed.
        /// </summary>
        protected override IEnumerable<IDaemonStageProcess> CreateProcessByFile(
            IDaemonProcess pProcess,
            IContextBoundSettingsStore pSettings,
            DaemonProcessKind pProcessKind, ICSharpFile pFile)
        {
            return new[] {new TestMissingProcess(pProcess)};
        }
    }
}