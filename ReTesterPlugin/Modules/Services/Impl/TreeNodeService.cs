using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace ReTesterPlugin.Modules.Services.Impl
{
    public class TreeNodeService : iTreeNodeService
    {
        /// <summary>
        /// Checks if the node is an identifier for a class, and returns the declaration if it is.
        /// </summary>
        public IClassDeclaration isClassIdentifier(ITreeNode pNode)
        {
            return isType(pNode, CSharpTokenType.IDENTIFIER)
                ? pNode.Parent as IClassDeclaration
                : null;
        }

        /// <summary>
        /// Checks if a node is a type of C# token.
        /// </summary>
        public bool isType(ITreeNode pNode, TokenNodeType pToken)
        {
            return pNode != null && pNode.NodeType == pToken;
        }
    }
}