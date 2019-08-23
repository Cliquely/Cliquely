using System;
using System.Runtime.Serialization;

namespace Cliquely.Exceptions
{
	class CliquesNotFoundException : Exception
	{
		public uint Gene { get; set; }

		public CliquesNotFoundException() { }

		public CliquesNotFoundException(uint gene, string message) : base(message) { }

		public CliquesNotFoundException(uint gene, string message, Exception innerException) : base(message, innerException) { }

		protected CliquesNotFoundException(uint gene, SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
