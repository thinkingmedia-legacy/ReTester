using System;
using System.Linq;

namespace ReTesterPlugin.Features.Naming
{
    public class UnitTestNaming : iTypeNaming
    {
        /// <summary>
        /// This will drop the first namespace (the project default)
        /// </summary>
        public string NameSpace(string pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }

            return string.Join(".", pNameSpace.Split(new[] {'.'}).Skip(1));
        }

        /// <summary>
        /// Append the word "Test"
        /// </summary>
        public string Identifier(string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            return string.Format("{0}Test", pName);
        }
    }
}