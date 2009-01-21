namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
	/// <summary>
	/// DataType에 대한 요약 설명입니다.
	/// </summary>
	public enum MpType
	{
		Binary = 0,
		Byte = 1,
//		SByte = 2,	// SByte 형식은 CLS 규격이 아닙니다. CLS 규격 대체 형식은 Int16입니다. 
		Int16 = 2,
		Int32 = 3,
		Int64 = 4,
//		UInt16 = 5,
//		UInt32 = 6,
//		UInt64 = 7,
		String = 5
	}

}
