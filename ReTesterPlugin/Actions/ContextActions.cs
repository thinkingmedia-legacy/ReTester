using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.Util;
using ReTesterPlugin.Actions.Bulbs;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions
{
    [ContextAction(Description = "My context action does ...", Name = "My Context Action", Group = "C#")]
    public sealed class ContextActions : IContextAction
    {
        /// <summary>
        /// A list of all actions.
        /// </summary>
        private readonly List<IBulbAction> _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can
        /// be injected in this constructor.
        /// </summary>
        public ContextActions(ICSharpContextActionDataProvider pProvider)
        {
            _items = new List<IBulbAction>
                     {
                         new CreateFile<IClassDeclaration>(pProvider, "unit test", FeaturesService.UnitTests),
                         new CreateFile<IClassDeclaration>(pProvider, "mock object", FeaturesService.MockObjects),
                         new CreateFile<IInterfaceDeclaration>(pProvider, "mock interface", FeaturesService.MockInterfaces),

                         new OpenFile<IClassDeclaration>(pProvider, "unit test", FeaturesService.UnitTests),
                         new OpenFile<IClassDeclaration>(pProvider, "mocked object", FeaturesService.MockObjects),
                         new OpenFile<IInterfaceDeclaration>(pProvider, "mocked interface", FeaturesService.MockInterfaces)
                     };
        }

        /// <summary>
        /// I use empty Text to indicate an action is not available.
        /// </summary>
        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return (from item in _items
                    where !string.IsNullOrWhiteSpace(item.Text)
                    select item).ToContextAction();
        }

        /// <summary>
        /// I use empty Text to indicate an action is not available.
        /// </summary>
        public bool IsAvailable(IUserDataHolder pCache)
        {
            return _items.Count(pItem=>!string.IsNullOrWhiteSpace(pItem.Text)) > 0;
        }
    }
}