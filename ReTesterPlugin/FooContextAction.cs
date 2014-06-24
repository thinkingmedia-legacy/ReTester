using System;
using System.Collections.Generic;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReTesterPlugin
{
    [ContextAction(Description = "My context action does ...",
        Group = "C#",
        Name = "My Context Action")]
    public sealed class FooContextAction : IContextAction
    {
        private readonly IBulbAction[] _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can
        /// be injected in this constructor.
        /// </summary>
        public FooContextAction(ICSharpContextActionDataProvider pProvider)
        {
            _items = new IBulbAction[] {new MyBulbItem(pProvider)};
        }

        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return _items.ToContextAction();
        }

        public bool IsAvailable(IUserDataHolder pCache)
        {
            // Availability code may be optimized but for most cases can be as simple as follow:
            return _items.Length > 0;
        }
    }

    public class MyBulbItem : BulbActionBase
    {
        private readonly ICSharpContextActionDataProvider _provider;
        private ICSharpFile _sharpFile;

        public override string Text
        {
            get { return "Foo test action"; }
        }

        public MyBulbItem(ICSharpContextActionDataProvider pProvider)
        {
            _provider = pProvider;
        }

        protected override void ExecuteBeforePsiTransaction(ISolution pSolution, IProjectModelTransactionCookie pCookie, IProgressIndicator pProgress)
        {
            // try to add a new file
            IProject project = _provider.PsiFile.GetProject();
            IProjectFile newFile = AddNewItemUtil.AddFile(project, "foo.cs");
            IPsiSourceFile sourceFile = newFile.ToSourceFile();
            _sharpFile = (ICSharpFile)sourceFile.GetPrimaryPsiFile();

            base.ExecuteBeforePsiTransaction(pSolution, pCookie, pProgress);
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            // put transacted steps here
            // use 'provider' field to get currently selected element

            // this code works fine
            CSharpElementFactory factory = CSharpElementFactory.GetInstance(_provider.PsiModule);
            ICSharpFile tmpFile = factory.CreateFile("public class FooTest {}");
            _sharpFile.AddTypeDeclarationAfter(tmpFile.TypeDeclarations[0], null);
            //_provider.PsiFile.AddTypeDeclarationAfter(tmpFile.TypeDeclarations[0], null);

            return textControl=>
                   {
                       // put post-transaction steps here
                       // if there are none, you can replace this with 'return null'
                   };
        }
    }
}