using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;

namespace ReTesterPlugin.Modules
{
    public interface iUnitTestService
    {
        /// <summary>
        /// Creates the unit test for a class.
        /// </summary>
        bool Create(IClassDeclaration pClass, IPsiModule pModule);

        /// <summary>
        /// Checks if a unit test exists for a class.
        /// </summary>
        bool Exists(IClassDeclaration pClass);

        /// <summary>
        /// Opens the unit test for a class.
        /// </summary>
        void Open(IClassDeclaration pClass);
    }
}