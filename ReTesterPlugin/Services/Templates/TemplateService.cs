using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services.Templates
{
    public static class TemplateService
    {
        public static readonly iTemplateProvider UnitTest = new UnitTestTemplate();
        public static readonly iTemplateProvider MockObject = new MockObjectTemplate();

        /// <summary>
        /// Creates the unit test file for a class declaration.
        /// </summary>
        public static ICSharpFile Create<TType>([NotNull] IProject pProject,
                                         [NotNull] TType pType,
                                         [NotNull] iTypeNaming pNaming,
                                         [NotNull] iTemplateProvider pTemplate)
            where TType : class, ITreeNode, ICSharpTypeDeclaration
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }
            if (pNaming == null)
            {
                throw new ArgumentNullException("pNaming");
            }
            if (pTemplate == null)
            {
                throw new ArgumentNullException("pTemplate");
            }

            if (TestProjectService.Exists(pType, pNaming)
                || !ClassService.isSafelyPublic(pType.ModifiersList))
            {
                return null;
            }

            string nameSpc = pNaming.NameSpace(pType.OwnerNamespaceDeclaration.DeclaredName);
            string filename = pNaming.Identifier(pType.NameIdentifier.Name);

            NustacheData data = pTemplate.GetData(pType, pProject.Name + "." + nameSpc, filename);
            string sourceCode = ResourceService.ReadAsString(typeof(TemplateService), pTemplate.GetTemplate());
            sourceCode = Nustache.Core.Render.StringToString(sourceCode, data);

            try
            {
                return ProjectService.AddFile<ICSharpFile>(pProject, nameSpc, filename + ".cs", sourceCode);
            }
            catch (IsFalseException)
            {
                return null;
            }
        }
    }
}