using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperToolKit.Editors;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Modules;
using ReSharperToolKit.Modules.Factories;
using ReSharperToolKit.Modules.Services;

namespace ReTesterPlugin.Modules.Impl
{
    public class UnitTestService : iUnitTestService
    {
        private readonly iNamingService _namingService;

        private readonly iProjectService _projectService;

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
            string nameSpc = _namingService.NameSpaceToTestNameSpace(pClass.OwnerNamespaceDeclaration.DeclaredName);
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
            [NotNull] iProjectService pProjectService,
            [NotNull] iNamingService pNamingService)
        {
            _testProjectService = pTestProjectService;
            _projectService = pProjectService;
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
        public bool Create([NotNull] IClassDeclaration pClass, IPsiModule pModule)
        {
            if (Exists(pClass))
            {
                return true;
            }

            IProject testProject = _testProjectService.getProject(pClass.GetProject());
            string unitTest = _namingService.ClassNameToTest(pClass.NameIdentifier.Name);
            string nameSpc = _namingService.NameSpaceToTestNameSpace(pClass.OwnerNamespaceDeclaration.DeclaredName);

            IProjectFolder folder = _projectService.getFolder(testProject, nameSpc);

            IProjectFile newFile = AddNewItemUtil.AddFile(folder, unitTest + ".cs");
            ICSharpFile icSharpFile = _projectService.getFileAs<ICSharpFile>(newFile);
            if (icSharpFile == null)
            {
                return false;
            }

            iElementEditorFactory editors = Locator.Get<iElementEditorFactory>();
            iClassEditor classEditor =
                editors.CreateSourceEditor(CSharpElementFactory.GetInstance(pModule), icSharpFile).AddClass(unitTest);
            return classEditor != null;
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