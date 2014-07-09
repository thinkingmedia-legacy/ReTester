using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester.Whens;

namespace ReTester
{
    public class DoesThis
    {
        /// <summary>
        /// The message
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Constructor
        /// </summary>
        public DoesThis(string pMessage)
        {
            _message = pMessage;
        }

        /// <summary>
        /// When the test is run this exception is expected.
        /// </summary>
        public iWhenAction Throws<TException>() 
            where TException : Exception
        {
            return new WhenThrows<TException>(_message);
        }

        /// <summary>
        /// When the function is called this type is returned.
        /// </summary>
        public TType Returns<TType>(Func<object> pFunc)
        {
            object value = pFunc();
            Assert.IsNotNull(value, _message);
            Assert.IsInstanceOfType(value, typeof(TType), _message);
            return (TType)value;
        }

        /// <summary>
        /// Creates a condition based upon the values in an array.
        /// </summary>
        public AreAll<TType> These<TType>(params TType[] pArrayValues)
        {
            return new AreAll<TType>(_message, pArrayValues);
        }
    }
}