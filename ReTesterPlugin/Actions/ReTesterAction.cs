using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using ReSharperToolKit.Actions;

namespace ReTesterPlugin.Actions
{
    public abstract class ReTesterAction : ClassActionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ReTesterAction(ICSharpContextActionDataProvider pProvider)
            : base(pProvider)
        {
        }
    }
}