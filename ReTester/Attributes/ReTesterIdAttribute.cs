using System;

namespace ReTester.Attributes
{
    /// <summary>
    /// Associates an ID with a class.
    /// </summary>
    public class ReTesterIdAttribute : Attribute
    {
        /// <summary>
        /// The unique ID
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pID">The ID to assigned to the class.</param>
        public ReTesterIdAttribute(string pID)
        {
            _id = new Guid(pID);
        }
    }
}