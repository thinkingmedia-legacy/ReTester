using System;

namespace ReTester.Attributes
{
    /// <summary>
    /// Associates an ID with a class.
    /// </summary>
    public class UnitTestAttribute : Attribute
    {
        /// <summary>
        /// The unique ID
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pID">The ID to assigned to the class.</param>
        public UnitTestAttribute(string pID)
        {
            _id = new Guid(pID);
        }
    }
}