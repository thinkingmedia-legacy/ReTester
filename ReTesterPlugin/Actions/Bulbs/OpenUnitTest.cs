﻿using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class OpenUnitTest : BaseAction<IClassDeclaration>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenUnitTest(ICSharpContextActionDataProvider pProvider)
            : base(pProvider, ePROJECT_SCOPE.SOURCE)
        {
        }

        /// <summary>
        /// Open the file.
        /// </summary>
        protected override Action<ITextControl> Action(ISolution pSolution,
                                                       IProgressIndicator pProgress,
                                                       IProject pProject,
                                                       IClassDeclaration pType,
                                                       ICSharpIdentifier pId)
        {
            FilesService.Open(pType, NamingService.TestNaming);
            return null;
        }

        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected override string getText(IProject pProject, IClassDeclaration pType, ICSharpIdentifier pId)
        {
            return string.Format("Open unit test {0}.cs [ReTester]", NamingService.TestNaming.Identifier(pId.Name));
        }

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected override bool isEnabled(IProject pProject, IClassDeclaration pType, ICSharpIdentifier pId)
        {
            return FilesService.Exists(pType, NamingService.TestNaming);
        }
    }
}