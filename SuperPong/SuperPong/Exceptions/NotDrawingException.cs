using System;
namespace SuperPong.Exceptions
{
    public class NotDrawingException : Exception
    {
        public NotDrawingException() : base("Cannot call end() without calling begin() first.")
        {
        }
    }
}
