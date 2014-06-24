using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;

namespace ReTesterPlugin.Modules
{
    public interface iUnitTestService
    {
        /// <summary>
        /// Prepares the unit test file outside of a PSI transaction
        /// </summary>
        ICSharpFile PreCreate(IClassDeclaration pClass, IPsiModule pModule);

        /// <summary>
        /// Creates the contents of the unit test file.
        /// </summary>
        bool Create(ICSharpFile pFile, IClassDeclaration pClass, IPsiModule pModule);

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