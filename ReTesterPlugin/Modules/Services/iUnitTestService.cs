using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;

namespace ReTesterPlugin.Modules.Services
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
        bool Create(IClassDeclaration pClass, IPsiModule pModule);

        /// <summary>
        /// Opens the unit test for a class.
        /// </summary>
        void Open(IClassDeclaration pClass);
    }
}