namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
	/// <summary>
	/// DataType�� ���� ��� �����Դϴ�.
	/// </summary>
	public enum MpType
	{
		Binary = 0,
		Byte = 1,
//		SByte = 2,	// SByte ������ CLS �԰��� �ƴմϴ�. CLS �԰� ��ü ������ Int16�Դϴ�. 
		Int16 = 2,
		Int32 = 3,
		Int64 = 4,
//		UInt16 = 5,
//		UInt32 = 6,
//		UInt64 = 7,
		String = 5
	}

}
