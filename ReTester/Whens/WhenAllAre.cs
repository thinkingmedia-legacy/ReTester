using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReTester.Whens
{
    /// <summary>
    /// Compares the return values with the values.
    /// </summary>
    public class WhenAllAre<TType, TRetType> : iWhenFunc<TType,TRetType>
    {
        /// <summary>
        /// The test message
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// The argument values
        /// </summary>
        private readonly List<TType> _values;

        /// <summary>
        /// The expected return value
        /// </summary>
        private readonly TRetType _retValue;

        /// <summary>
        /// Constructor
        /// </summary>
        public WhenAllAre(string pMessage, List<TType> pValues, TRetType pRetValue)
        {
            _message = pMessage;
            _values = pValues;
            _retValue = pRetValue;
        }

        /// <summary>
        /// Executes the action passing a value as an argument.
        /// </summary>
        public iWhenFunc<TType, TRetType> When(Func<TType, TRetType> pDo)
        {
            _values.ForEach(pValue=>
                            {
                                TRetType ret = pDo(pValue);
                                Assert.AreEqual(_retValue, ret, string.Format("{0}: {1}", _message, pValue.ToString()));
                            });

            return new WhenAllAre<TType, TRetType>(_message, _values, _retValue);
        }
    }
}