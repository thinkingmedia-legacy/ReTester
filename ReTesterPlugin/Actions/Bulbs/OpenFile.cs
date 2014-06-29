using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class OpenFile<TType> : BaseAction<TType>
        where TType : class, ITreeNode, ICSharpTypeDeclaration
    {
        /// <summary>
        /// The name of thing this opens
        /// </summary>
        private readonly string _thing;

        /// <summary>
        /// The current naming convention
        /// </summary>
        private readonly iTypeNaming _naming;

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenFile(ICSharpContextActionDataProvider pProvider, string pThing, iTypeNaming pNaming)
            : base(pProvider, ePROJECT_SCOPE.SOURCE)
        {
            _thing = pThing;
            _naming = pNaming;
        }

        /// <summary>
        /// Open the file.
        /// </summary>
        protected override Action<ITextControl> Action(ISolution pSolution,
                                                       IProgressIndicator pProgress,
                                                       IProject pProject,
                                                       TType pType,
                                                       ICSharpIdentifier pId)
        {
            FilesService.Open(pType, _naming);
            return null;
        }


        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected override string getText(IProject pProject, TType pType, ICSharpIdentifier pId)
        {
            return string.Format("Open {0} {1}.cs [ReTester]", _thing, _naming.Identifier(pId.Name));
        }

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected override bool isEnabled(IProject pProject, TType pType, ICSharpIdentifier pId)
        {
            return FilesService.Exists(pType, _naming);
        }
    }
}