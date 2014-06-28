using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperToolKit.Services;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services.Templates
{
    public class MockObjectTemplate : iTemplateProvider
    {
        public NustacheData GetData(ITreeNode pType, string pNameSpc, string pClassName)
        {
            IInterfaceDeclaration intrface = (IInterfaceDeclaration)pType;

            NustacheData data = new NustacheData();
            data["namespace"] = pNameSpc;
            data["mock_class"] = pClassName;
            data["src_class"] = intrface.NameIdentifier.Name;

            data["Using"] = new List<NustacheData>();
            string srcNamespace = ClassService.getFullNameSpace(intrface);
            data.List("Using").Add(new NustacheData { { "value", srcNamespace } });

            return data;
        }

        public string GetTemplate()
        {
            return "ReTesterPlugin.Templates.MockObject.mustache";
        }
    }
}