using System;
namespace SuperPong.Exceptions
{
    public class AlreadyDrawingException : Exception
    {
        public AlreadyDrawingException() : base("Cannot call begin() without calling end() first.")
        {
        }
    }
}
