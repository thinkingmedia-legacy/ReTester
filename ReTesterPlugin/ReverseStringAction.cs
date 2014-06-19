using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Macros;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReTesterPlugin
{
    /// <summary>
    /// This is an example context action. The test project demonstrates tests for
    /// availability and execution of this action.
    /// </summary>
    [ContextAction(Name = "ReverseString", Description = "Reverses a string", Group = "C#")]
    public class ReverseStringAction : ContextActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICSharpContextActionDataProvider _provider;

        /// <summary>
        /// The literal object in the source code.
        /// </summary>
        private ILiteralExpression _stringLiteral;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReverseStringAction(ICSharpContextActionDataProvider pRovider)
        {
            _provider = pRovider;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsAvailable(IUserDataHolder pCache)
        {
            ILiteralExpression literal = _provider.GetSelectedElement<ILiteralExpression>(true, true);
            if (literal == null || !literal.IsConstantValue() || !literal.ConstantValue.IsString())
            {
                return false;
            }

            string s = literal.ConstantValue.Value as string;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            _stringLiteral = literal;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
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

            return null;
        }

        /// <summary>
        /// The action menu description.
        /// </summary>
        public override string Text
        {
            get { return "Reverse string"; }
        }
    }
}