using JetBrains.ActionManagement;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateMockObjects")]
    public class CreateMockObjects : CreateFile<ICSharpFile, IClassDeclaration>
    {
        public CreateMockObjects()
            : base(FeaturesService.MockObjects)
        {
        }
    }
}