using System;
using System.Runtime.Serialization;

namespace Cliquely.Exceptions
{
	class CliquesNotFoundException : Exception
	{
		public uint Gene { get; }

		public CliquesNotFoundException(uint gene)
		{
			Gene = gene;
		}

		public CliquesNotFoundException(uint gene, string message) : base(message)
		{
			Gene = gene;
		}

		public CliquesNotFoundException(uint gene, string message, Exception innerException) : base(message, innerException)
		{
			Gene = gene;
		}

		protected CliquesNotFoundException(uint gene, SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Gene = gene;
		}
	}
}
