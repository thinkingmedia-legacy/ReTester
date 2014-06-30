using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Features.Filtering
{
    public class PublicOnly : iFilter
    {
        private static readonly TokenNodeType[] _noIncomplete =
        {
            CSharpTokenType.ABSTRACT_KEYWORD,
            CSharpTokenType.INTERNAL_KEYWORD,
            CSharpTokenType.PARTIAL_KEYWORD
        };

        private static readonly TokenNodeType[] _onlyPublic =
        {
            CSharpTokenType.PUBLIC_KEYWORD
        };

        public bool isMatch(IModifiersList pModifiers)
        {
            return FilterService.Check(pModifiers, _onlyPublic, _noIncomplete);
        }
    }
}