using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace ReTesterPlugin.Services
{
    public static class FilterService
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

        /// <summary>
        /// Checks that a list of modifiers match the rules.
        /// </summary>
        private static bool Check([CanBeNull] IModifiersList pModifiers,
                                  [NotNull] IEnumerable<TokenNodeType> pMustHave,
                                  [NotNull] IEnumerable<TokenNodeType> pMustNotHave)
        {
            if (pModifiers == null)
            {
                return false;
            }
            if (pMustHave == null)
            {
                throw new ArgumentNullException("pMustHave");
            }
            if (pMustNotHave == null)
            {
                throw new ArgumentNullException("pMustNotHave");
            }

            List<TokenNodeType> types = pModifiers.Modifiers.Select(pMod=>pMod.GetTokenType()).ToList();

            return !types.Any(pTypeB=>pMustNotHave.Any(pTypeA=>pTypeA == pTypeB))
                   && types.Any(pTypeB=>pMustHave.All(pTypeA=>pTypeA == pTypeB));
        }

        /// <summary>
        /// Class must be abstract without being partial.
        /// </summary>
        public static bool isAbstract([CanBeNull] IModifiersList pModifiers)
        {
            return Check(pModifiers, _mustBeAbstract, _notHidden);
        }

        /// <summary>
        /// Class must be public without being partial.
        /// </summary>
        public static bool isPublic([CanBeNull] IModifiersList pModifiers)
        {
            return Check(pModifiers, _onlyPublic, _noIncomplete);
        }
    }
}