using JetBrains.ReSharper.Psi.Tree;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services.Templates
{
    public interface iTemplateProvider
    {
        NustacheData GetData(ITreeNode pClass, string pNameSpc, string pClassName);
        string GetTemplate();
    }
}