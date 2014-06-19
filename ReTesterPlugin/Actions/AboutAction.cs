using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;

namespace ReTesterPlugin.Actions
{
    [ActionHandler("ReTesterPlugin.About")]
    public class AboutAction : IActionHandler
    {
        /// <summary>
        /// Updates action visual presentation. If presentation.Enabled is set to false, Execute
        /// will not be called.
        /// </summary>
        /// <param name="pContext">DataContext</param>
        /// <param name="pResentation">presentation to update</param>
        /// <param name="pNextUpdate">delegate to call</param>
        public bool Update(IDataContext pContext, ActionPresentation pResentation, DelegateUpdate pNextUpdate)
        {
            // return true or false to enable/disable this action
            return true;
        }

        /// <summary>
        /// Executes action. Called after Update, that set ActionPresentation.Enabled to true.
        /// </summary>
        /// <param name="pContext">DataContext</param>
        /// <param name="pNextExecute">delegate to call</param>
        public void Execute(IDataContext pContext, DelegateExecute pNextExecute)
        {
            MessageBox.Show(
                "ReTester\nMathew Foscarini, http://www.ThinkingMedia.ca\n\nKeep your unit tests and source code in sync.",
                "About ReTester",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}