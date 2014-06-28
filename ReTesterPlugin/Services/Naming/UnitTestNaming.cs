namespace ReTesterPlugin.Services.Naming
{
    public class UnitTestNaming : iTypeNaming
    {
        public string NameSpace(string pNameSpace)
        {
            return pNameSpace;
        }

        public string Identifier(string pName)
        {
            return string.Format("{0}Test", pName);
        }
    }
}
