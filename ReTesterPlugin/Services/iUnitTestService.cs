using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReTesterPlugin.Services
{
    public interface iUnitTestService
    {
        /// <summary>
        /// Checks if a unit test exists for a class.
        /// </summary>
        bool Exists(IClassDeclaration pClass);

        /// <summary>
        /// Creates the unit test for a class.
        /// </summary>
        void Create(IClassDeclaration pClass);

        /// <summary>
        /// Opens the unit test for a class.
        /// </summary>
        void Open(IClassDeclaration pDecl);
    }
}