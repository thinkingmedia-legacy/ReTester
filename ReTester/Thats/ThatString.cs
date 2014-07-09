using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReTester.Thats
{
    /// <summary>
    /// Defines rules associated with an exception.
    /// </summary>
    internal class ThatString : iThat, iThatRule
    {
        /// <summary>
        /// The description of that thing.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// The thing being tested.
        /// </summary>
        private readonly iWhenAction _whenAction;

        /// <summary>
        /// The rule function
        /// </summary>
        private RuleFunc _rule;

        /// <summary>
        /// Constructor
        /// </summary>
        private ThatString(ThatString pThat)
        {
            _whenAction = pThat._whenAction;
            _message = pThat._message;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThatString(iWhenAction pWhenAction, string pMessage)
        {
            _whenAction = pWhenAction;
            _message = pMessage;
        }

        /// <summary>
        /// Returns back to the chain of when things happen.
        /// </summary>
        public iWhenAction Then()
        {
            return _whenAction;
        }

        /// <summary>
        /// That object must contain
        /// </summary>
        public iThat Contains(string pValue)
        {
            _rule = pStr=>pStr.Contains(pValue);

            return new ThatString(this);
        }

        /// <summary>
        /// That object must start with
        /// </summary>
        public iThat StartsWith(string pValue)
        {
            _rule = pStr=>pStr.StartsWith(pValue);

            return new ThatString(this);
        }

        /// <summary>
        /// That object must end with
        /// </summary>
        public iThat EndsWith(string pValue)
        {
            _rule = pStr=>pStr.EndsWith(pValue);

            return new ThatString(this);
        }

        /// <summary>
        /// That object is equal to or same as
        /// </summary>
        public iThat isSameAs(string pValue)
        {
            _rule = pStr=>pStr == pValue;

            return new ThatString(this);
        }

        /// <summary>
        /// Tests that a rule matches.
        /// </summary>
        public bool isMatch(string pValue)
        {
            Assert.IsNotNull(_rule);

            return _rule(pValue);
        }

        /// <summary>
        /// Defines the function used to perform a test.
        /// </summary>
        private delegate bool RuleFunc(string pValue);
    }
}