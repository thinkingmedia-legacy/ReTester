namespace ReTesterPlugin.Services.Naming
{
    public class MockObjectNaming : iTypeNaming
    {
        public string NameSpace(string pNameSpace)
        {
            return string.Format("Mock.{0}", pNameSpace);
        }

        public string Identifier(string pName)
        {
            // TODO: Strip the I
            return string.Format("Mock{0}", pName);
        }
    }
}