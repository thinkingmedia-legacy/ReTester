using JetBrains.ReSharper.Psi.Tree;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Features.Templates
{
    public interface iTemplateProvider
    {
        NustacheData GetData(ITreeNode pClass, string pNameSpc, string pClassName);
        string GetTemplate();
    }
}