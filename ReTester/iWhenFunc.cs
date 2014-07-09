using System;

namespace ReTester
{
    public interface iWhenFunc<out TType, in TRetType>
    {
        /// <summary>
        /// Executes the action passing a value as an argument.
        /// </summary>
        iWhenFunc<TType, TRetType> When(Func<TType, TRetType> pDo);
    }
}