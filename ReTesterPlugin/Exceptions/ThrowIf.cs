namespace ReTesterPlugin.Exceptions
{
    /// <summary>
    /// A convenience class for common things to check when evaluating a boolean function.
    /// </summary>
    public static class ThrowIf
    {
        /// <summary>
        /// Is the parameter False?
        /// </summary>
        /// <example cref="ReTesterPlugin.Exceptions.IsFalseException">Thrown when condition is true.</example>
        public static void False(bool pValue, string pMessage = null)
        {
            if (pValue == false)
            {
                throw new IsFalseException(pMessage);
            }
        }

        /// <summary>
        /// Is the parameter Null?
        /// </summary>
        /// <example cref="ReTesterPlugin.Exceptions.IsFalseException">Thrown when condition is true.</example>
        public static T Null<T>(T pValue, string pMessage = null) where T : class
        {
            if (pValue == null)
            {
                throw new IsFalseException(pMessage);
            }
            return pValue;
        }

        /// <summary>
        /// Is the parameter True?
        /// </summary>
        /// <example cref="ReTesterPlugin.Exceptions.IsFalseException">Thrown when condition is true.</example>
        public static void True(bool pValue, string pMessage = null)
        {
            if (pValue)
            {
                throw new IsFalseException(pMessage);
            }
        }
    }
}