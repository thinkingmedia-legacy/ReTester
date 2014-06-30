using System;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using ReTesterPlugin.Services;

namespace ReTesterPlugin.Actions.Bulbs
{
    public abstract class BaseAction<TType> : BulbActionBase
        where TType : class, ITreeNode, ICSharpTypeDeclaration
    {
        /// <summary>
        /// Holds the selected context
        /// </summary>
        private readonly ICSharpContextActionDataProvider _provider;

        /// <summary>
        /// Displays the create message
        /// </summary>
        public sealed override string Text
        {
            get
            {
                IProject sourceProject;
                IProject testProject;
                ICSharpIdentifier id;
                TType actionType;

                if (!GetCurrentSelection(out sourceProject, out testProject, out id, out actionType))
                {
                    return "";
                }

                if (!isEnabled(sourceProject, testProject, actionType, id))
                {
                    return "";
                }

                string str = getText(sourceProject, testProject, actionType, id);
                return string.IsNullOrWhiteSpace(str) ? "" : str;
            }
        }

        /// <summary>
        /// Attempts to get the current class identifier from the current project. The active project is restricted to the current
        /// scope. If anything doesn't match then this returns false.
        /// </summary>
        private bool GetCurrentSelection(out IProject pSourceProject,
                                         out IProject pTestProject,
                                         out ICSharpIdentifier pID,
                                         out TType pActionType)
        {
            pID = _provider.GetSelectedElement<ICSharpIdentifier>(true, true);
            pActionType = _provider.GetSelectedElement<TType>(true, true);
            pSourceProject = FilesService.getSourceProject(_provider.Project);
            pTestProject = FilesService.getTestProject(_provider.Project);

            if (pSourceProject == null || pTestProject == null)
            {
                return false;
            }

            return pID != null
                   && pActionType != null
                   && pActionType.NameIdentifier == pID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseAction(ICSharpContextActionDataProvider pProvider)
        {
            _provider = pProvider;
        }

        /// <summary>
        /// Called only if everything is verified to be in a valid state.
        /// </summary>
        protected virtual Action<ITextControl> Action(ISolution pSolution,
                                                      IProgressIndicator pProgress,
                                                      IProject pSourceProject,
                                                      IProject pTestProject,
                                                      TType pType,
                                                      ICSharpIdentifier pId)
        {
            return null;
        }

        /// <summary>
        /// Called only if everything is verified to be in a valid state.
        /// </summary>
        protected virtual void BeforeAction(IProject pSourceProject,
                                            IProject pTestProject,
                                            TType pType,
                                            ICSharpIdentifier pId)
        {
        }

        /// <summary>
        /// Can't add new files to a project during a PSI transaction. So it's done here before the transaction is started.
        /// </summary>
        protected sealed override void ExecuteBeforePsiTransaction(ISolution pSolution,
                                                                   IProjectModelTransactionCookie pCookie,
                                                                   IProgressIndicator pProgress)
        {
            IProject sourceProject;
            IProject testProject;
            ICSharpIdentifier id;
            TType actionType;

            if (!GetCurrentSelection(out sourceProject, out testProject, out id, out actionType))
            {
                return;
            }

            BeforeAction(sourceProject, testProject, actionType, id);
        }

        protected sealed override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution,
                                                                             IProgressIndicator pProgress)
        {
            IProject sourceProject;
            IProject testProejct;
            ICSharpIdentifier id;
            TType actionType;

            return GetCurrentSelection(out sourceProject, out testProejct, out id, out actionType)
                ? Action(pSolution, pProgress, sourceProject, testProejct, actionType, id)
                : null;
        }

        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected abstract string getText(IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId);

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected abstract bool isEnabled(IProject pSourceProject, IProject pTestProject, TType pType, ICSharpIdentifier pId);
    }
}