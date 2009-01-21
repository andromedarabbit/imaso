using System;
using System.Security.Permissions;

namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
	public enum MemoryPacketErrorCode
	{
		Undefined,
		InvalidPacketType,
		InvalidEncodingType,
		InvalidParameterValue,
		FixedSize,
		Overflow
	}

	/// <summary>
	/// BaseApplicationException에 대한 요약 설명입니다.
	/// </summary>
	[Serializable]
	public class MemoryPacketException : Exception
	{
		/// <summary>
		/// Constructor with no params.
		/// </summary>
		public MemoryPacketException() : base()
		{
		}

		/// <summary>
		/// Constructor allowing the Message property to be set.
		/// </summary>
		/// <param name="message">String setting the message of the exception.</param>
		public MemoryPacketException(string message) : base(message) 
		{
		}

		/// <summary>
		/// Constructor allowing the Message and InnerException property to be set.
		/// </summary>
		/// <param name="message">String setting the message of the exception.</param>
		/// <param name="inner">Sets a reference to the InnerException.</param>
		public MemoryPacketException(string message,Exception inner) : base(message, inner)
		{
		}

		public MemoryPacketException(MemoryPacketErrorCode errorCode)  
		{
			error_code = errorCode;
		}

		public MemoryPacketException(string message,MemoryPacketErrorCode errorCode) : base(message)
		{
			error_code = errorCode;
		}

		public MemoryPacketException(string message,Exception inner,MemoryPacketErrorCode errorCode) : base(message, inner)
		{
			error_code = errorCode;
		}

        private MemoryPacketErrorCode error_code = MemoryPacketErrorCode.Undefined;

		public MemoryPacketErrorCode ErrorCode
		{
			get
			{
				return error_code;
			}
		}

		// override GetObjectData to serialize state data
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData (info, context);
		}

	}
}
