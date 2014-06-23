using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace ReTesterPlugin.Modules.Services
{
    public interface iTreeNodeService
    {
        /// <summary>
        /// Checks if the node is an identifier for a class, and returns the declaration if it is.
        /// </summary>
        IClassDeclaration isClassIdentifier(ITreeNode pNode);

        /// <summary>
        /// Checks if a node is a type of C# token.
        /// </summary>
        bool isType(ITreeNode pNode, TokenNodeType pToken);
    }
}