using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReTesterPlugin.Services
{
    public static class NamingService
    {
        /// <summary>
        /// The regex for matching a test project name.
        /// </summary>
        private readonly static Regex _testProjectRegex = new Regex("^(.*)Tests$");

        /// <summary>
        /// The regex for matching a unit test.
        /// </summary>
        private readonly static Regex _unitTestRegex = new Regex("^(.*)Test$");

        /// <summary>
        /// True if the project name follows a test project naming convention.
        /// </summary>
        public static bool isTestProjectName(string pName)
        {
            return _testProjectRegex.IsMatch(pName);
        }

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
        public static string ProjectToTestProject([NotNull] string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            return isTestProjectName(pName) 
                ? pName 
                : string.Format("{0}Tests", pName);
        }

        /// <summary>
        /// Converts a test project name into the source project name.
        /// </summary>
        public static string TestProjectToProject([NotNull] string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            if (!isTestProjectName(pName))
            {
                return pName;
            }

            Match m = _testProjectRegex.Match(pName);
            return m.Groups[0].Value;
        }
    }
}