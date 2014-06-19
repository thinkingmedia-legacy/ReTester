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
        public static void False(bool pValue)
        {
            if (pValue == false)
            {
                throw new IsFalseException();
            }
        }

        /// <summary>
        /// Is the parameter Null?
        /// </summary>
        /// <example cref="ReTesterPlugin.Exceptions.IsFalseException">Thrown when condition is true.</example>
        public static T Null<T>(T pValue) where T : class
        {
            if (pValue == null)
            {
                throw new IsFalseException();
            }
            return pValue;
        }

        /// <summary>
        /// Is the parameter True?
        /// </summary>
        /// <example cref="ReTesterPlugin.Exceptions.IsFalseException">Thrown when condition is true.</example>
        public static void True(bool pValue)
        {
            if (pValue)
            {
                throw new IsFalseException();
            }
        }
    }
}