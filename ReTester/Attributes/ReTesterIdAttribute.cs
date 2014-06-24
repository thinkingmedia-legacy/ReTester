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
        public string ID { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReTesterIdAttribute()
        {
            ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pID">The ID to assigned to the class.</param>
        public ReTesterIdAttribute(string pID)
        {
            ID = pID;
        }
    }
}