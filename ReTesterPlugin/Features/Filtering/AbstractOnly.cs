using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Features.Filtering
{
    public class AbstractOnly : iFilter
    {
        private static readonly TokenNodeType[] _mustBeAbstract =
        {
            CSharpTokenType.PUBLIC_KEYWORD,
            CSharpTokenType.ABSTRACT_KEYWORD
        };

        private static readonly TokenNodeType[] _notHidden =
        {
            CSharpTokenType.INTERNAL_KEYWORD,
            CSharpTokenType.PARTIAL_KEYWORD
        };

        public bool isMatch(IModifiersList pModifiers)
        {
            return FilterService.Check(pModifiers, _mustBeAbstract, _notHidden);
        }
    }
}