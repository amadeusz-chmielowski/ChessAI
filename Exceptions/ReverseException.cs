using System;
using System.Runtime.Serialization;

namespace ChessAI
{
	[Serializable]
	internal class ReverseException : Exception
	{
		public ReverseException()
		{
		}

		public ReverseException(string message) : base(message)
		{
		}

		public ReverseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ReverseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}