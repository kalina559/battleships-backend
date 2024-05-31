namespace Battleships.Core.Exceptions
{
    public class NullGameStateException : Exception
    {
        public NullGameStateException()
        {
        }

        public NullGameStateException(string message)
            : base(message)
        {
        }

        public NullGameStateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}