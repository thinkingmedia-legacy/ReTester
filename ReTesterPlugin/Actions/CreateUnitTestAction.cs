using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using ReTesterPlugin.Exceptions;
using ReTesterPlugin.Services;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ReTesterPlugin.Actions
{
    /// <summary>
    /// Enables an action of a class to create a matching unit test.
    /// </summary>
    [ContextAction(Name = "CreateUnitTest", Description = "Creates a unit test for this class", Group = "C#")]
    public class CreateUnitTestAction : ContextActionBase
    {
        /// <summary>
        /// Not null if editing a C# file.
        /// </summary>
        private readonly ICSharpContextActionDataProvider _provider;

        /// <summary>
        /// The recommended filename.
        /// </summary>
        private string _testName;

        /// <summary>
        /// Naming services
        /// </summary>
        private readonly iNamingService _naming;

        /// <summary>
        /// The theme
        /// </summary>
        private readonly iAppTheme _theme;

        /// <summary>
        /// The current project
        /// </summary>
        private IProject _project;

        /// <summary>
        /// The class namespace
        /// </summary>
        private string _namespace;

        /// <summary>
        /// Displays the recommended filename.
        /// </summary>
        /// <summary>
        /// The action menu description.
        /// </summary>
        public override string Text
        {
            get { return _theme.ActionText(string.Format("Create unit test {0}.cs", _testName ?? "")); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateUnitTestAction(ICSharpContextActionDataProvider pRovider)
        {
            _provider = pRovider;
            _testName = null;

            _naming = Locator.Get<iNamingService>();
            _theme = Locator.Get<iAppTheme>();
        }

        /// <summary>
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            try
            {
                string testName = _naming.ProjectToTestProject(_project.Name);
                IProject testProject = ThrowIf.Null(pSolution.GetProjectByName(testName), string.Format("Project [{0}] not found.", testName));
                ThrowIf.False(testProject.IsOpened,string.Format("Test project [{0}] is not open.", testName));

                FileSystemPath path = testProject.ProjectFileLocation;

                List<string> tmp = new List<string> {path.Directory.FullPath};
                tmp.AddRange(_namespace.Split(new[] {'.'}));
                tmp.Add(_testName + ".cs");

                string targetFile = tmp.Join(@"\");
                FileSystemPath file = FileSystemPath.TryParse(targetFile);
                ThrowIf.True(file == FileSystemPath.Empty,string.Format("Invalid path {0}", targetFile));

                ThrowIf.True(file.Exists == FileSystemPath.Existence.File, string.Format("File already exists {0}", targetFile));

                //IList<IProjectItem> items = testProject.GetSubItems(_testName);
                IList<IProjectItem> items = testProject.GetSubItems();

                return null;
            }
            catch (IsFalseException e)
            {
                if (!string.IsNullOrWhiteSpace(e.Message))
                {
                    MessageBox.Show(
                        e.Message,
                        "Create Unit Test",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }

                return null;
            }

            return null;
        }

        /// <summary>
        /// Checks if the cursor is on a Class identifier for a declaration.
        /// </summary>
        public override bool IsAvailable(IUserDataHolder pCache)
        {
            try
            {
                ITreeNode node = ThrowIf.Null(_provider.SelectedElement);
                ThrowIf.False(node.IsValid());

                IClassDeclaration declaration = ThrowIf.Null(node.Parent as IClassDeclaration);
                ICSharpNamespaceDeclaration namespaceDeclaration = ThrowIf.Null(declaration.OwnerNamespaceDeclaration);

                TokenNodeType type = ThrowIf.Null(node.NodeType as TokenNodeType);
                ThrowIf.True(type != CSharpTokenType.IDENTIFIER);

                _namespace = namespaceDeclaration.ShortName;
                _testName = _naming.ClassNameToTest(node.GetText());

                _project = ThrowIf.Null(node.GetProject());
            }
            catch (IsFalseException)
            {
                return false;
            }

            return true;
        }
    }
}