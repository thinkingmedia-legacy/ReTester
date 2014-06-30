using System;

namespace ReTesterPlugin.Features.Naming
{
    public class MockInterfaceNaming : MockNamingBase
    {
        /// <summary>
        /// Prefix classes with Mock, but drop I for interfaces.
        /// </summary>
        public override string Identifier(string pName)
        {
            if (pName == null)
            {
                throw new ArgumentNullException("pName");
            }

            // Drop first letter if I
            string str = pName.ToLower().StartsWith("i")
                ? pName.Substring(1)
                : pName;

            return string.Format("Mock{0}", str);
        }
    }
}