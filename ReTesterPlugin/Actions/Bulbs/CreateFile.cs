using System;
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
    public class CreateFile<TType> : BaseFile<TType>
        where TType : class, ITreeNode, ICSharpTypeDeclaration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateFile(ICSharpContextActionDataProvider pProvider,
                          string pNoun, iFeatureType pFeatureType)
            : base(pProvider, "Create", pNoun, pFeatureType)
        {
        }

        /// <summary>
        /// Just opens the new file.
        /// </summary>
        protected override Action<ITextControl> Action(ISolution pSolution, IProgressIndicator pProgress, IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId)
        {
            return pTextControl=>FilesService.Open(pType, FeatureType.Naming);
        }

        /// <summary>
        /// Creates the new unit test file outside of the PSI transaction.
        /// </summary>
        protected override void BeforeAction(IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId)
        {
            TemplateService.Create(pTestProject, pType, FeatureType);
        }

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected override bool isEnabled(IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId)
        {
            return FeatureType.Filter.isMatch(pType.ModifiersList) 
                && !FilesService.Exists(pType, FeatureType.Naming);
        }
    }
}