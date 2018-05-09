using System;
using System.Runtime.Serialization;

namespace ChessAI
{
    [Serializable]
    internal class DeclareDrawException : Exception
    {
        public DeclareDrawException()
        {
        }

        public DeclareDrawException(string message) : base(message)
        {
        }

        public DeclareDrawException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeclareDrawException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}