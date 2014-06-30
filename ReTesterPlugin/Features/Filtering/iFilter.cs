using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReTesterPlugin.Features.Filtering
{
    /// <summary>
    /// Provides matching rules for a C sharp type.
    /// </summary>
    public interface iFilter
    {
        bool isMatch(IModifiersList pModifiers);
    }
}