using System;
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

        private readonly iNamingService _naming;

        /// <summary>
        /// Displays the recommended filename.
        /// </summary>
        /// <summary>
        /// The action menu description.
        /// </summary>
        public override string Text
        {
            get { return _naming.ActionText(string.Format("Create unit test {0}.cs", _testName ?? "")); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateUnitTestAction(ICSharpContextActionDataProvider pRovider)
        {
            _provider = pRovider;
            _testName = null;

            _naming = Locator.Get<iNamingService>();
        }

        /// <summary>
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
/*
            CSharpElementFactory factory = CSharpElementFactory.GetInstance(_provider.PsiModule);

            string stringValue = _stringLiteral.ConstantValue.Value as string;
            if (stringValue == null)
            {
                return null;
            }

            char[] chars = stringValue.ToCharArray();
            Array.Reverse(chars);
            ICSharpExpression newExpr = factory.CreateExpressionAsIs("\"" + new string(chars) + "\"");
            _stringLiteral.ReplaceBy(newExpr);
*/

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
                ThrowIf.False(node.Parent is IClassDeclaration);

                TokenNodeType type = ThrowIf.Null(node.NodeType as TokenNodeType);
                ThrowIf.True(type != CSharpTokenType.IDENTIFIER);

                _testName = _naming.ClassNameToTest(node.GetText());
            }
            catch (IsFalseException)
            {
                return false;
            }

            return true;
        }
    }
}