using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester.Thats;

namespace ReTester.Whens
{
    /// <summary>
    /// Handles the testing of something that should throw an exception.
    /// </summary>
    internal class WhenThrows<TException> : iWhenAction
        where TException : Exception
    {
        /// <summary>
        /// The description of it.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// A list of conditions for that exception.
        /// </summary>
        private readonly List<iThatRule> _that;

        /// <summary>
        /// Constructor
        /// </summary>
        public WhenThrows(string pMessage)
        {
            _message = pMessage;
            _that = new List<iThatRule>();
        }

        /// <summary>
        /// Test that the action throws an expected exception.
        /// </summary>
        public iWhenAction When(Action pDo)
        {
            try
            {
                pDo();
            }
            catch (TException e)
            {
                foreach (iThatRule that in _that.Where(pThat=>!pThat.isMatch(e.Message)))
                {
                    Assert.Fail(_message);
                }
                return this;
            }
            catch (Exception e)
            {
                Assert.Fail("Unhandled exception {0}", e.Message);
            }

            Assert.Fail(_message);

            return this;
        }

        /// <summary>
        /// Returns a reference to an object that has been associated with the current test.
        /// </summary>
        public iThat That()
        {
            ThatString rule = new ThatString(this, _message);
            _that.Add(rule);
            return rule;
        }

        /// <summary>
        /// Continues the chain of tests
        /// </summary>
        public iWhenAction Then()
        {
            return this;
        }
    }
}