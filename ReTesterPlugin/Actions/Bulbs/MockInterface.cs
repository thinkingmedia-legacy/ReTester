using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReTesterPlugin.Services;
using ReTesterPlugin.Services.Naming;

namespace ReTesterPlugin.Actions.Bulbs
{
    public class MockInterface : BaseAction<IInterfaceDeclaration>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MockInterface(ICSharpContextActionDataProvider pProvider)
            : base(pProvider, ePROJECT_SCOPE.SOURCE)
        {
        }

        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected override string getText(IProject pProject, IInterfaceDeclaration pType, ICSharpIdentifier pId)
        {
            return string.Format("Create mock {0}.cs [ReTester]", NamingService.MockInterfaces.Identifier(pId.Name));
        }

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected override bool isEnabled(IProject pProject, IInterfaceDeclaration pType, ICSharpIdentifier pId)
        {
            return !FilesService.Exists(pType, NamingService.MockInterfaces);
        }
    }
}