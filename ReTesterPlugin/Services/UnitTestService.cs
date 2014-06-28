using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using Nustache.Core;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;
using ReTesterPlugin.Services.Naming;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services
{
    public static class UnitTestService
    {
        /// <summary>
        /// Calculates the full path to the unit test file from a class identifier.
        /// </summary>
        private static string getUnitTestFile(
            [NotNull] IProject pProject,
            [NotNull] ICSharpTypeDeclaration pClass)
        {
            string nameSpc = NamingService.NameSpaceToTestNameSpace(pClass.OwnerNamespaceDeclaration.DeclaredName);
            string unitTest = NamingService.NameToTestName(pClass.NameIdentifier.Name);

            FileSystemPath path = pProject.ProjectFileLocation;
            List<string> tmp = new List<string> {path.Directory.FullPath};
            tmp.AddRange(nameSpc.Split(new[] {'.'}));
            tmp.Add(unitTest + ".cs");
            return tmp.Join(@"\");
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