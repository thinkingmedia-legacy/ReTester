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
        /// The scope of this action
        /// </summary>
        private readonly ePROJECT_SCOPE _scope;

        /// <summary>
        /// Displays the create message
        /// </summary>
        public sealed override string Text
        {
            get
            {
                IProject project;
                ICSharpIdentifier id;
                TType actionType;

                if (!GetCurrentSelection(out project, out id, out actionType))
                {
                    return "";
                }

                if (!isEnabled(project, actionType, id))
                {
                    return "";
                }

                string str = getText(project, actionType, id);
                return string.IsNullOrWhiteSpace(str) ? "" : str;
            }
        }

        /// <summary>
        /// Check if a project is with in scope.
        /// </summary>
        private static bool InScope(ePROJECT_SCOPE pScope, IProject pProject)
        {
            switch (pScope)
            {
                case ePROJECT_SCOPE.SOURCE:
                    if (!FilesService.isSourceProject(pProject))
                    {
                        return false;
                    }
                    break;
                case ePROJECT_SCOPE.TEST:
                    if (!FilesService.isTestProject(pProject))
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Attempts to get the current class identifier from the current project. The active project is restricted to the current
        /// scope. If anything doesn't match then this returns false.
        /// </summary>
        private bool GetCurrentSelection(out IProject pProject, out ICSharpIdentifier pID, out TType pActionType)
        {
            pID = _provider.GetSelectedElement<ICSharpIdentifier>(true, true);
            pActionType = _provider.GetSelectedElement<TType>(true, true);
            pProject = getProject();

            if (pProject == null)
            {
                return false;
            }

            return pID != null
                   && pActionType != null
                   && pActionType.NameIdentifier == pID;
        }

        /// <summary>
        /// Gets the current project as long as it's with in the current scope.
        /// </summary>
        private IProject getProject()
        {
            IProject project = _provider.Project;
            return !InScope(_scope, project) ? null : project;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseAction(ICSharpContextActionDataProvider pProvider, ePROJECT_SCOPE pScope)
        {
            _provider = pProvider;
            _scope = pScope;
        }

        protected virtual Action<ITextControl> Action(ISolution pSolution, IProgressIndicator pProgress,
                                                      IProject pProject, TType pType, ICSharpIdentifier pId)
        {
            return null;
        }

        protected virtual void BeforeAction(IProject pProject, TType pType, ICSharpIdentifier pId)
        {
        }

        /// <summary>
        /// Can't add new files to a project during a PSI transaction. So it's done here before the transaction is started.
        /// </summary>
        protected sealed override void ExecuteBeforePsiTransaction(ISolution pSolution,
                                                                   IProjectModelTransactionCookie pCookie,
                                                                   IProgressIndicator pProgress)
        {
            IProject project;
            ICSharpIdentifier id;
            TType actionType;

            if (!GetCurrentSelection(out project, out id, out actionType))
            {
                return;
            }

            BeforeAction(project, actionType, id);
        }

        protected sealed override Action<ITextControl> ExecutePsiTransaction(ISolution pSolution,
                                                                             IProgressIndicator pProgress)
        {
            IProject project;
            ICSharpIdentifier id;
            TType actionType;

            return GetCurrentSelection(out project, out id, out actionType) 
                ? Action(pSolution, pProgress, project, actionType, id) 
                : null;
        }

        /// <summary>
        /// Gets the text for this action
        /// </summary>
        protected abstract string getText(IProject pProject, TType pType, ICSharpIdentifier pId);

        /// <summary>
        /// Checks if the action is currently enabled.
        /// </summary>
        protected abstract bool isEnabled(IProject pProject, TType pType, ICSharpIdentifier pId);
    }
}