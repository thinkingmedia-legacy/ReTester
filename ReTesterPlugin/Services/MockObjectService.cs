using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReTesterPlugin.Services
{
    public static class MockObjectService
    {
        public static void Create([NotNull] IProject pProject, [NotNull] IInterfaceDeclaration pType)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }
        }
    }
}