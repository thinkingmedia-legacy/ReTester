﻿using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using JetBrains.IDE;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperToolKit.Editors;
using ReSharperToolKit.Exceptions;

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
            string unitTest = NamingService.ClassNameToTestName(pClass.NameIdentifier.Name);

            FileSystemPath path = pProject.ProjectFileLocation;
            List<string> tmp = new List<string> {path.Directory.FullPath};
            tmp.AddRange(nameSpc.Split(new[] {'.'}));
            tmp.Add(unitTest + ".cs");
            return tmp.Join(@"\");
        }

        /// <summary>
        /// Creates the contents of the unit test file.
        /// </summary>
        public static bool Create([NotNull] ICSharpFile pFile, [NotNull] IClassDeclaration pClass,
                                  [NotNull] IPsiModule pModule)
        {
            if (pFile == null)
            {
                throw new ArgumentNullException("pFile");
            }
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }
            if (pModule == null)
            {
                throw new ArgumentNullException("pModule");
            }

            string unitTest = NamingService.ClassNameToTestName(pClass.NameIdentifier.Name);

            ClassEditor classEditor =
                new SourceEditor(CSharpElementFactory.GetInstance(pModule), pFile).AddClass(unitTest);

            return classEditor != null;
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
                IProject testProject = ThrowIf.Null(TestProjectService.getProject(project));

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
                IProject testProject = ThrowIf.Null(TestProjectService.getProject(project));

                string outFile = getUnitTestFile(testProject, pClass);

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