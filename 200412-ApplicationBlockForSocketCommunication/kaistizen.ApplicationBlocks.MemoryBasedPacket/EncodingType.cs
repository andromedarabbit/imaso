namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
	/// <summary>
	/// EncodingType에 대한 요약 설명입니다.
	/// </summary>
	public enum EncodingType
	{
		// 시스템의 현재 ANSI 코드 페이지에 대한 인코딩을 가져옵니다
		Default = 0,	
		// big-endian 바이트 순서로 유니코드 형식에 대한 인코딩을 가져옵니다
		BigEndianUnicode = 1,
		// little-endian 바이트 순서로 유니코드 형식에 대한 인코딩을 가져옵니다.
		Unicode = 2,
		// UTF-7 형식에 대한 인코딩을 가져옵니다.
		UTF7 = 3,
		// UTF-8 형식에 대한 인코딩을 가져옵니다.
		UTF8 = 4,
		// ASCII 형식에 대한 인코딩을 가져옵니다.
		ASCII = 5
	}
}
