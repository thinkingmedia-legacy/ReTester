using System;

namespace ReTester.Attributes
{
    public class ReTesterUnitAttribute : Attribute
    {
        /// <summary>
        /// The ID to match in the source code.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Should this unit test be moved to match the source class namespace.
        /// </summary>
        public bool AutoMove { get; set; }

        /// <summary>
        /// Should this unit test be renamed to match the source class.
        /// </summary>
        public bool MatchName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReTesterUnitAttribute()
        {
            ID = Guid.NewGuid().ToString();
            AutoMove = false;
            MatchName = false;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ReTesterUnitAttribute(string pID, bool pAutoMove = true, bool pMatchName = true)
        {
            ID = pID;
            AutoMove = pAutoMove;
            MatchName = pMatchName;
        }
    }
}