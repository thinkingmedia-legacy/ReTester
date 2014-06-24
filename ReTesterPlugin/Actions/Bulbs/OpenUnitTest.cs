using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.TextControl;
using JetBrains.Util;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Bulbs
{
    /// <summary>
    /// Enables an action of a class to create a matching unit test.
    /// </summary>
    //[ContextAction(Name = "OpenUnitTest", Description = "Opens an existing unit test for a class", Group = "C#")]
    public class OpenUnitTest : ReTesterAction
    {
        /// <summary>
        /// Displays the recommended filename.
        /// </summary>
        public override string Text
        {
            get
            {
                return "Open unit test XXXX.cs";
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenUnitTest(ICSharpContextActionDataProvider pProvider)
            : base(pProvider)
        {
        }

        /// <summary>
        /// Creates the new unit test file.
        /// </summary>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution, IProgressIndicator pProgress)
        {
            if (SelectedClass != null)
            {
                UnitTestService.Open(SelectedClass.Decl);
            }
            return null;
        }

        /// <summary>
        /// Can a unit test be created for this class?
        /// </summary>
        protected override bool isAvailableForClass(IUserDataHolder pCache)
        {
            IProject testProejct = TestProjectService.getProject(Provider.Project);
            return testProejct != null && UnitTestService.Exists(SelectedClass.Decl);
        }
    }
}