using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using Nustache.Core;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services.Templates
{
    public static class TemplateService
    {
        public static readonly iTemplateProvider MockInterface = new MockObjectTemplate<IInterfaceDeclaration>();
        public static readonly iTemplateProvider MockObject = new MockObjectTemplate<IClassDeclaration>();
        public static readonly iTemplateProvider UnitTest = new UnitTestTemplate();

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

            if (FilesService.Exists(pType, pNaming)
                || !FilterService.isPublic(pType.ModifiersList))
            {
                return null;
            }

            string nameSpc = pNaming.NameSpace(pType.OwnerNamespaceDeclaration.DeclaredName);
            string filename = pNaming.Identifier(pType.NameIdentifier.Name);

            NustacheData data = pTemplate.GetData(pType, pProject.Name + "." + nameSpc, filename);
            string sourceCode = ResourceService.ReadAsString(typeof (TemplateService), pTemplate.GetTemplate());
            sourceCode = Render.StringToString(sourceCode, data);

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