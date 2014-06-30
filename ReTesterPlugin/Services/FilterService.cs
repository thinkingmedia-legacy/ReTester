using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace ReTesterPlugin.Services
{
    public static class FilterService
    {
        /// <summary>
        /// Checks that a list of modifiers match the rules.
        /// </summary>
        public static bool Check([CanBeNull] IModifiersList pModifiers,
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

            return !pMustNotHave.Any(types.Contains) && pMustHave.All(types.Contains);
        }
    }
}