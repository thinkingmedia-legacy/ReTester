using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.Util;
using ReTesterPlugin.Actions.Bulbs;

namespace ReTesterPlugin.Actions
{
    [ContextAction(Description = "My context action does ...", Name = "My Context Action", Group = "C#")]
    public sealed class ContextActions : IContextAction
    {
        private readonly List<IBulbAction> _items;

        /// <summary>
        /// For languages other than C# any inheritor of <see cref="IContextActionDataProvider"/> can
        /// be injected in this constructor.
        /// </summary>
        public ContextActions(ICSharpContextActionDataProvider pProvider)
        {
            _items = new List<IBulbAction>
                     {
                         new CreateUnitTest(pProvider),
                         new OpenUnitTest(pProvider)
                     };
        }

        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return (from item in _items
                    where !string.IsNullOrWhiteSpace(item.Text)
                    select item).ToContextAction();
        }

        public bool IsAvailable(IUserDataHolder pCache)
        {
            return _items.Count(pItem=>!string.IsNullOrWhiteSpace(pItem.Text)) > 0;
        }
    }
}