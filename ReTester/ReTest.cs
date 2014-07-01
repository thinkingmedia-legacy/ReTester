using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReTester
{
    /// <summary>
    /// A test running tool.
    /// </summary>
    public static class ReTest
    {
        /// <summary>
        /// Starts a chainable description of what a test does.
        /// </summary>
        /// <param name="pMessage">A description of what the test does.</param>
        /// <returns>The thing it does.</returns>
        public static DoesThis It(string pMessage)
        {
            return new DoesThis(pMessage);
        }
    }
}