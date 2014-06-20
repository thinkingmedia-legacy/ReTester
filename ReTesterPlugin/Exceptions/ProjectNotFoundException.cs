using System;

namespace ReTesterPlugin.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string pMessage)
            : base(pMessage)
        {
            
        }
    }
}