namespace ReTesterPlugin.Editors
{
    public interface iClassEditor
    {
        /// <summary>
        /// Adds a new attribute to the class declaration.
        /// </summary>
        /// <param name="pAttribute">The attribute as source code excluding the []</param>
        void AddAttribute(string pAttribute);
    }
}