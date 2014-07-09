using System;
using System.Collections.Generic;
using System.Linq;
using ReTester.Whens;

namespace ReTester
{
    public class AreAll<TType>
    {
        /// <summary>
        /// The message for this test.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// The values for all conditions
        /// </summary>
        private readonly List<TType> _values;

        /// <summary>
        /// Constructor
        /// </summary>
        public AreAll(string pMessage, IEnumerable<TType> pValues)
        {
            _message = pMessage;
            _values = pValues.ToList();
        }

        /// <summary>
        /// Checks that all values return false.
        /// </summary>
        public iWhenFunc<TType, bool> AreAllFalse()
        {
            return new WhenAllAre<TType,bool>(_message, _values, false);
        }

        /// <summary>
        /// Checks that all values return true.
        /// </summary>
        public iWhenFunc<TType, bool> AreAllTrue()
        {
            return new WhenAllAre<TType,bool>(_message, _values, true);
        }

        /// <summary>
        /// Checks that all values are equal.
        /// </summary>
        public iWhenFunc<TType,TExpect> AreAllEqualTo<TExpect>(TExpect pValue)
        {
            return new WhenAllAre<TType,TExpect>(_message, _values, pValue);
        }

        /// <summary>
        /// Checks that all values are not equal.
        /// </summary>
        public iWhenFunc<TType,TExpect> AreAllNotEqualTo<TExpect>(TExpect pValue)
        {
            //return new WhenAllAreNot<TType,TExpect>(_message, _values, pValue);
            throw new NotImplementedException();
        }
    }
}