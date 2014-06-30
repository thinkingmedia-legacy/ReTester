using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using Nustache.Core;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Features;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services
{
    public static class TemplateService
    {
        /// <summary>
        /// Creates the unit test file for a class declaration.
        /// </summary>
        public static ICSharpFile Create<TType>([NotNull] IProject pProject,
                                                [NotNull] TType pType, 
                                                [NotNull] iFeatureType pFeatureType)
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
            if (pFeatureType == null)
            {
                throw new ArgumentNullException("pFeatureType");
            }

            if (FilesService.Exists(pType, pFeatureType.Naming)
                || !pFeatureType.Filter.isMatch(pType.ModifiersList))
            {
                return null;
            }

            string nameSpc = pFeatureType.Naming.NameSpace(pType.OwnerNamespaceDeclaration.DeclaredName);
            string filename = pFeatureType.Naming.Identifier(pType.NameIdentifier.Name);

            NustacheData data = pFeatureType.Template.GetData(pType, pProject.Name + "." + nameSpc, filename);
            string sourceCode = ResourceService.ReadAsString(typeof (TemplateService), pFeatureType.Template.GetTemplate());
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