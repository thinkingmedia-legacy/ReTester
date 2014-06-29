using JetBrains.Annotations;

namespace ReTesterPlugin.Services.Naming
{
    public interface iTypeNaming
    {
        string Identifier([NotNull] string pName);
        string NameSpace([NotNull] string pNameSpace);
    }
}