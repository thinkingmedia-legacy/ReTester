using JetBrains.ActionManagement;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Menus
{
    [ActionHandler("CreateUnitTests")]
    public class CreateUnitTests : CreateFile<ICSharpFile, IClassDeclaration>
    {
        public CreateUnitTests()
            : base(FeaturesService.UnitTests)
        {
        }
    }
}