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
        public iWhen Throws<TException>() 
            where TException : Exception
        {
            return new WhenException<TException>(_message);
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
    }
}