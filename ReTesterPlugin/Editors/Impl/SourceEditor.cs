using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReTesterPlugin.Editors.Impl
{
    public class SourceEditor : iSourceEditor
    {
        /// <summary>
        /// An element factory
        /// </summary>
        private readonly CSharpElementFactory _factory;

        /// <summary>
        /// The source code file.
        /// </summary>
        private readonly ICSharpFile _file;

        /// <summary>
        /// Constructor
        /// </summary>
        public SourceEditor(CSharpElementFactory pFactory, ICSharpFile pFile)
        {
            _factory = pFactory;
            _file = pFile;
        }

        /// <summary>
        /// Adds a using declaration to the using list.
        /// </summary>
        public void AddUsing(string pNameSpace)
        {
            IUsingDirective directive = _factory.CreateUsingDirective(string.Format("using {0}", pNameSpace));
            _file.AddImport(directive);
        }
    }
}