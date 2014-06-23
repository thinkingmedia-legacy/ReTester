using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;

namespace ReTesterPlugin.Editors.Impl
{
    public class ClassEditor : iClassEditor
    {
        /// <summary>
        /// The class being edited.
        /// </summary>
        private readonly IClassDeclaration _class;

        /// <summary>
        /// C sharp element factory
        /// </summary>
        private readonly CSharpElementFactory _factory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClassEditor(CSharpElementFactory pFactory, IClassDeclaration pClass)
        {
            _class = pClass;
            _factory = pFactory;
        }

        /// <summary>
        /// Adds a new attribute to the class declaration.
        /// </summary>
        /// <param name="pAttribute">The attribute as source code excluding the []</param>
        public void AddAttribute(string pAttribute)
        {
            ICSharpTypeMemberDeclaration tmpClass = _factory.CreateTypeMemberDeclaration(string.Format("[{0}] class C{{}}", pAttribute));
            _class.AddAttributeAfter(tmpClass.Attributes[0], null);
        }
    }
}