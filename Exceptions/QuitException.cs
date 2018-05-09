using System;
using System.Runtime.Serialization;

namespace ChessAI
{
	[Serializable]
	internal class QuitException : Exception
	{
		public QuitException()
		{
		}

		public QuitException(string message) : base(message)
		{
		}

		public QuitException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected QuitException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}