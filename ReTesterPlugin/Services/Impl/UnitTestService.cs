using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReTesterPlugin.Exceptions;

namespace ReTesterPlugin.Services.Impl
{
    public class UnitTestService : iUnitTestService
    {
        private readonly iNamingService _namingService;

        /// <summary>
        /// Services for managing the test project.
        /// </summary>
        private readonly iTestProjectService _testProjectService;

        /// <summary>
        /// Calculates the full path to the unit test file from a class identifier.
        /// </summary>
        private string getUnitTestFile(
            [NotNull] IProject pProject,
            [NotNull] IClassDeclaration pClass)
        {
            string nameSpc = pClass.OwnerNamespaceDeclaration.ShortName;
            string unitTest = _namingService.ClassNameToTest(pClass.NameIdentifier.Name);

            FileSystemPath path = pProject.ProjectFileLocation;
            List<string> tmp = new List<string> {path.Directory.FullPath};
            tmp.AddRange(nameSpc.Split(new[] {'.'}));
            tmp.Add(unitTest + ".cs");
            return tmp.Join(@"\");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UnitTestService(
            [NotNull] iTestProjectService pTestProjectService,
            [NotNull] iNamingService pNamingService)
        {
            _testProjectService = pTestProjectService;
            _namingService = pNamingService;
        }

        /// <summary>
        /// Checks if a unit test exists for a class.
        /// </summary>
        public bool Exists([NotNull] IClassDeclaration pClass)
        {
            IProject testProject = _testProjectService.getProject(pClass.GetProject());
            if (testProject == null)
            {
                return false;
            }

            string nameSpc = pClass.OwnerNamespaceDeclaration.ShortName;
            string unitTest = _namingService.ClassNameToTest(pClass.NameIdentifier.Name);

            FileSystemPath path = testProject.ProjectFileLocation;
            List<string> tmp = new List<string> {path.Directory.FullPath};
            tmp.AddRange(nameSpc.Split(new[] {'.'}));
            tmp.Add(unitTest + ".cs");
            string targetFile = tmp.Join(@"\");

            return File.Exists(targetFile);
        }

        /// <summary>
        /// Creates the unit test for a class.
        /// </summary>
        public void Create([NotNull] IClassDeclaration pClass)
        {
            if (Exists(pClass))
            {
                return;
            }

            IProject project = _testProjectService.getProject(pClass.GetProject());
            string outFile = getUnitTestFile(project, pClass);
        }

        /// <summary>
        /// Opens the unit test for a class.
        /// </summary>
        public void Open(IClassDeclaration pClass)
        {
            try
            {
                IProject project = ThrowIf.Null(pClass.GetProject());
                IProject testProject = ThrowIf.Null(_testProjectService.getProject(project));

                string outFile = getUnitTestFile(testProject, pClass);

                ISolution solution = ThrowIf.Null(project.GetSolution());
                EditorManager editor = EditorManager.GetInstance(solution);

                editor.OpenFile(FileSystemPath.Parse(outFile), true, TabOptions.Default);
            }
            catch (IsFalseException)
            {
            }
            catch (InvalidPathException)
            {
            }
        }
    }
}