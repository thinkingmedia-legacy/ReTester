using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReTesterPlugin.Features;

namespace ReTesterPlugin.Actions.Bulbs
{
    public abstract class BaseFile<TType> : BaseAction<TType>
        where TType : class, ITreeNode, ICSharpTypeDeclaration
    {
        /// <summary>
        /// The features this file is based upon.
        /// </summary>
        protected readonly iFeatureType FeatureType;

        /// <summary>
        /// The name of thing this opens
        /// </summary>
        private readonly string _noun;

        /// <summary>
        /// </summary>
        private readonly string _verb;

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseFile(ICSharpContextActionDataProvider pProvider, string pVerb, string pNoun, iFeatureType pFeatureType)
            : base(pProvider)
        {
            _noun = pNoun;
            FeatureType = pFeatureType;
            _verb = pVerb;
        }

        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected override string getText(IProject pSourceProject, IProject pTestProject, TType pType,
                                          ICSharpIdentifier pId)
        {
            return string.Format("{0} {1} {2}.cs [ReTester]", _verb, _noun, FeatureType.Naming.Identifier(pId.Name));
        }
    }
}