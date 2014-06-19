using System;

namespace ReTesterPlugin.Exceptions
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(Type pType)
            : base(string.Format("Service [{0}] was not found.", pType.Name))
        {
            
        }
    }
}