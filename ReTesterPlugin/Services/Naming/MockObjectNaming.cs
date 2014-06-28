using System;

namespace ReTesterPlugin.Services.Naming
{
    public class MockObjectNaming : MockNamingBase
    {
        /// <summary>
        /// Prefix with Mock
        /// </summary>
        public override string Identifier(string pName)
        {
            return string.Format("Mock{0}", pName);
        }
    }
}