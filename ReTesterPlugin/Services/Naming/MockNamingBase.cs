using System;
using System.Linq;

namespace ReTesterPlugin.Services.Naming
{
    public abstract class MockNamingBase : iTypeNaming
    {
        /// <summary>
        /// This will replace the first namespace with the word "Mock"
        /// </summary>
        public string NameSpace(string pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }

            string str = string.Join(".", pNameSpace.Split(new[] {'.'}).Skip(1));
            return string.Format("Mock.{0}", str);
        }

        /// <summary>
        /// Let derived classes handle it.
        /// </summary>
        public abstract string Identifier(string pName);
    }
}