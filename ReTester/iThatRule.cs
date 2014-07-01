namespace ReTester
{
    internal interface iThatRule
    {
        /// <summary>
        /// Tests that a rule matches.
        /// </summary>
        bool isMatch(string pValue);
    }
}