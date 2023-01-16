using System;

namespace Models.Misc
{
    public class AkianaException : Exception
    {
        public AkianaException(string message) : base(message)
        {
        }
    }
}