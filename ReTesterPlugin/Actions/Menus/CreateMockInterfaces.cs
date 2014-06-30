using JetBrains.ActionManagement;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateMockInterfaces")]
    public class CreateMockInterfaces : CreateFile<ICSharpFile, IInterfaceDeclaration>
    {
        public CreateMockInterfaces()
            : base(FeaturesService.MockInterfaces)
        {
        }
    }
}