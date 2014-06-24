using System;

namespace ReTester.Attributes
{
    public class ReTesterUnitAttribute : Attribute
    {
        /// <summary>
        /// The ID to match in the source code.
        /// </summary>
        private readonly string _id;

        /// <summary>
        /// Should this unit test be moved to match the source class namespace.
        /// </summary>
        private bool _autoMove;

        /// <summary>
        /// Should this unit test be renamed to match the source class.
        /// </summary>
        private bool _matchName;

        /// <summary>
        /// Constructor
        /// </summary>
        // ReSharper disable InconsistentNaming
        public ReTesterUnitAttribute(string ID, bool AutoMove = true, bool MatchName = true)
        {
            _id = ID;
            _autoMove = AutoMove;
            _matchName = MatchName;
        }
    }
}