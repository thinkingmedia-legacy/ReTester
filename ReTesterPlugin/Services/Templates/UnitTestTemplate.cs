using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharperToolKit.Services;
using ReTesterPlugin.Templates;

namespace ReTesterPlugin.Services.Templates
{
    public class UnitTestTemplate : iTemplateProvider
    {
        public NustacheData GetData(ITreeNode pType, string pNameSpc, string pClassName)
        {
            IClassDeclaration pClass = (IClassDeclaration)pType;

            NustacheData data = new NustacheData();
            data["namespace"] = pNameSpc;
            data["test_class"] = pClassName;
            data["src_class"] = pClass.NameIdentifier.Name;

            data["Using"] = new List<NustacheData>();
            string srcNamespace = ClassService.getFullNameSpace(pClass);
            data.List("Using").Add(new NustacheData { { "value", srcNamespace } });

            // Add these constructors
            data["Methods"] = new List<NustacheData>();
            for (int i = 0; i < Math.Max(1, pClass.ConstructorDeclarations.Count); i++)
            {
                data.List("Methods")
                    .Add(new NustacheData { { "name", String.Format("Constructor_{0}", i + 1) }, { "body", "" } });
            }

            // Add the methods (grouped by overloaded names)
            List<IGrouping<string, IMethodDeclaration>> methods = (from method in pClass.MethodDeclarations
                                                                   group method by method.NameIdentifier.Name
                                                                       into gMethods
                                                                       orderby gMethods.Key
                                                                       select gMethods).ToList();

            foreach (IGrouping<string, IMethodDeclaration> method in methods)
            {
                List<IMethodDeclaration> list = method.ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    data.List("Methods")
                        .Add(new NustacheData { { "name", String.Format("{0}_{1}", method.Key, i + 1) }, { "body", "" } });
                }
            }

            return data;
        }

        public string GetTemplate()
        {
            return "ReTesterPlugin.Templates.UnitTest.mustache";
        }
    }
}
