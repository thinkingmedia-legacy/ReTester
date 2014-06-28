using JetBrains.Annotations;

namespace ReTesterPlugin.Services.Naming
{
    public interface iTypeNaming
    {
        string NameSpace([NotNull] string pNameSpace);
        string Identifier([NotNull] string pName);
    }
}
