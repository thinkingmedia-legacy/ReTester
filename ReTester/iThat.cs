namespace ReTester
{
    /// <summary>
    /// Refers to an object that has been associated with the current test.
    /// </summary>
    public interface iThat : iThen
    {
        /// <summary>
        /// That object must contain
        /// </summary>
        iThat  Contains(string pValue);

        /// <summary>
        /// That object must start with
        /// </summary>
        iThat StartsWith(string pValue);

        /// <summary>
        /// That object must end with
        /// </summary>
        iThat EndsWith(string pValue);

        /// <summary>
        /// That object is equal to or same as
        /// </summary>
        iThat isSameAs(string pValue);
    }
}