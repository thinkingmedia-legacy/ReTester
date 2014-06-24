using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReTesterPlugin.Services
{
    public static class NamingService
    {
        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        public static string ClassNameToTestName([NotNull] string pClassName)
        {
            if (pClassName == null)
            {
                throw new ArgumentNullException("pClassName");
            }

            return string.Format("{0}Test", pClassName);
        }

        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        public static string ClassNameToTestName([NotNull] IClassDeclaration pClass)
        {
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }
            return ClassNameToTestName(pClass.NameIdentifier.Name);
        }

        /// <summary>
        /// Converts a namespace for a class into the namespace for the unit test.
        /// </summary>
        public static string NameSpaceToTestNameSpace([NotNull] string pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }

            // strip first namespace
            pNameSpace = pNameSpace.Substring(pNameSpace.IndexOf('.') + 1);
            // replace with Tests
            return string.Format("Tests.{0}", pNameSpace);
        }

        /// <summary>
        /// Converts a namespace for a class into the namespace for the unit test.
        /// </summary>
        public static string NameSpaceToTestNameSpace([NotNull] ICSharpNamespaceDeclaration pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            return NameSpaceToTestNameSpace(pNameSpace.DeclaredName);
        }

        /// <summary>
        /// Converts a project name into the test project.
        /// </summary>
        public static string ProjectToTestProject([NotNull] string pProject)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }

            return string.Format("{0}Tests", pProject);
        }
    }
}