using JetBrains.Annotations;

namespace ReTesterPlugin.Features.Naming
{
    public interface iTypeNaming
    {
        string Identifier([NotNull] string pName);
        string NameSpace([NotNull] string pNameSpace);
    }
}