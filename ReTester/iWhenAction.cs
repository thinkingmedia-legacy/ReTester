using System;

namespace ReTester
{
    public interface iWhenAction : iThen
    {
        /// <summary>
        /// Executes the action associated with the current scope.
        /// </summary>
        iWhenAction When(Action pDo);

        /// <summary>
        /// Returns a reference to an object that has been associated with the current test.
        /// </summary>
        iThat That();
    }
}