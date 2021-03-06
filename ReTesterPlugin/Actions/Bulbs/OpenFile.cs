﻿using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using ReTesterPlugin.Features;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class OpenFile<TType> : BaseFile<TType>
        where TType : class, ITreeNode, ICSharpTypeDeclaration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenFile(ICSharpContextActionDataProvider pProvider, string pThing, iFeatureType pFeatureType)
            : base(pProvider, "Open", pThing, pFeatureType)
        {
        }

        /// <summary>
        /// Open the file.
        /// </summary>
        protected override Action<ITextControl> Action(ISolution pSolution, IProgressIndicator pProgress, IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId)
        {
            FilesService.Open(pType, FeatureType.Naming);
            return null;
        }

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected override bool isEnabled(IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId)
        {
            return FeatureType.Filter.isMatch(pType.ModifiersList)
                && FilesService.Exists(pType, FeatureType.Naming);
        }
    }
}