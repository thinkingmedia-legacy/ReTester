using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using Nustache.Core;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services
{
    public static class UnitTestService
    {
        /// <summary>
        /// Creates the source code for a unit test file.
        /// </summary>
        private static string CreateSourceCode(IClassDeclaration pClass, string pNameSpc, string pUnitTest)
        {
            NustacheData data = new NustacheData();
            data["namespace"] = pNameSpc;
            data["classname"] = pUnitTest;

            data["Using"] = new List<NustacheData>();
            data.List("Using").Add(new NustacheData {{"namespace", "ReTester.Attributes"}});

            // Add these constructors
            data["Methods"] = new List<NustacheData>();
            for (int i = 0; i < pClass.ConstructorDeclarations.Count; i++)
            {
                data.List("Methods")
                    .Add(new NustacheData {{"name", string.Format("Construct_{0}", i)}, {"body", ""}});
            }

            // Add the methods (grouped by overloaded names)
            List<IGrouping<string, IMethodDeclaration>> methods = (from method in pClass.MethodDeclarations
                                                                   group method by method.NameIdentifier.Name
                                                                   into gMethods
                                                                   orderby gMethods.Key
                                                                   select gMethods).ToList();

            foreach (IGrouping<string, IMethodDeclaration> method in methods)
            {
                List<IMethodDeclaration> list = method.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    data.List("Methods")
                        .Add(new NustacheData {{"name", string.Format("{0}_{1}", method.Key, i + 1)}, {"body", ""}});
                }
            }

            string sourceCode = ResourceService.ReadAsString(typeof (UnitTestService),
                "ReTesterPlugin.Templates.UnitTest.mustache");
            sourceCode = Render.StringToString(sourceCode, data);
            return sourceCode;
        }

        /// <summary>
        /// Calculates the full path to the unit test file from a class identifier.
        /// </summary>
        private static string getUnitTestFile(
            [NotNull] IProject pProject,
            [NotNull] ICSharpTypeDeclaration pClass)
        {
            string nameSpc = NamingService.NameSpaceToTestNameSpace(pClass.OwnerNamespaceDeclaration.DeclaredName);
            string unitTest = NamingService.ClassNameToTestName(pClass.NameIdentifier.Name);

            FileSystemPath path = pProject.ProjectFileLocation;
            List<string> tmp = new List<string> {path.Directory.FullPath};
            tmp.AddRange(nameSpc.Split(new[] {'.'}));
            tmp.Add(unitTest + ".cs");
            return tmp.Join(@"\");
        }

        /// <summary>
        /// Creates the unit test file for a class declaration.
        /// </summary>
        public static ICSharpFile Create([NotNull] IProject pTestProject, [NotNull] IClassDeclaration pClass)
        {
            if (pTestProject == null)
            {
                throw new ArgumentNullException("pTestProject");
            }
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }

            string nameSpc = NamingService.NameSpaceToTestNameSpace(pClass.OwnerNamespaceDeclaration.DeclaredName);
            string unitTest = NamingService.ClassNameToTestName(pClass.NameIdentifier.Name);
            string sourceCode = CreateSourceCode(pClass, pTestProject.Name + "." + nameSpc, unitTest);

            try
            {
                IProjectFile projectFile =
                    ThrowIf.Null(ProjectService.AddFile(pTestProject, nameSpc, unitTest + ".cs", sourceCode));
                IPsiSourceFile sourceFile = ThrowIf.Null(projectFile.ToSourceFile());
                return ThrowIf.Null(sourceFile.GetPrimaryPsiFile() as ICSharpFile);
            }
            catch (IsFalseException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates unit tests for all the classes in a file.
        /// </summary>
        public static void Create(IProject pTestProject, ICSharpFile pCSharpFile)
        {
            List<IClassDeclaration> classes = (from node in pCSharpFile.EnumerateSubTree()
                                               let decl = node as IClassDeclaration
                                               where decl != null
                                               select decl).ToList();

            foreach (IClassDeclaration clss in classes)
            {
                Create(pTestProject, clss);
            }
        }

        /// <summary>
        /// Checks if a unit test exists for a class.
        /// </summary>
        public static bool Exists([NotNull] IClassDeclaration pClass)
        {
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }

            try
            {
                // TODO: This works by finding a file. It should search the project for the unit test declaration.

                IProject project = ThrowIf.Null(pClass.GetProject());
                IProject testProject = ThrowIf.Null(TestProjectService.getTestProject(project));

                string nameSpc = NamingService.NameSpaceToTestNameSpace(pClass.OwnerNamespaceDeclaration);
                string unitTest = NamingService.ClassNameToTestName(pClass);

                FileSystemPath path = testProject.ProjectFileLocation;
                string file = path.Directory.FullPath + @"\" + nameSpc.Replace(".", @"\") + @"\" + unitTest + ".cs";
                return File.Exists(file);
            }
            catch (IsFalseException)
            {
                return false;
            }
        }

        /// <summary>
        /// Opens the unit test for a class.
        /// </summary>
        public static bool Open([NotNull] IClassDeclaration pClass)
        {
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }

            try
            {
                IProject project = ThrowIf.Null(pClass.GetProject());
                IProject testProject = ThrowIf.Null(TestProjectService.getTestProject(project));

                string outFile = getUnitTestFile(testProject, pClass);
                ThrowIf.False(File.Exists(outFile));

                ISolution solution = ThrowIf.Null(project.GetSolution());
                EditorManager editor = EditorManager.GetInstance(solution);

                editor.OpenFile(FileSystemPath.Parse(outFile), true, TabOptions.Default);
            }
            catch (IsFalseException)
            {
                return false;
            }
            catch (InvalidPathException)
            {
                return false;
            }

            return true;
        }
    }
}