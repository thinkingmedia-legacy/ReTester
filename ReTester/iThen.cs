namespace ReTester
{
    public interface iThen
    {
        /// <summary>
        /// Continues the chain of tests
        /// </summary>
        iWhenAction Then();
    }
}