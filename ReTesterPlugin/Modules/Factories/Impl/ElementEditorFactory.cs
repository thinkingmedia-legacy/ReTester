using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using ReTesterPlugin.Editors;
using ReTesterPlugin.Editors.Impl;

namespace ReTesterPlugin.Modules.Factories.Impl
{
    public class ElementEditorFactory : iElementEditorFactory
    {
        /// <summary>
        /// Create an editor for class declarations.
        /// </summary>
        /// <param name="pFactory"></param>
        /// <param name="pClass"></param>
        public iClassEditor CreateClassEditor(CSharpElementFactory pFactory, IClassDeclaration pClass)
        {
            return new ClassEditor(pFactory, pClass);
        }

        /// <summary>
        /// Create an editor for source files.
        /// </summary>
        /// <param name="pFactory"></param>
        /// <param name="pFile"></param>
        public iSourceEditor CreateSourceEditor(CSharpElementFactory pFactory, ICSharpFile pFile)
        {
            return new SourceEditor(pFactory, pFile);
        }
    }
}