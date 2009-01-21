namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
	/// <summary>
	/// EncodingType�� ���� ��� �����Դϴ�.
	/// </summary>
	public enum EncodingType
	{
		// �ý����� ���� ANSI �ڵ� �������� ���� ���ڵ��� �����ɴϴ�
		Default = 0,	
		// big-endian ����Ʈ ������ �����ڵ� ���Ŀ� ���� ���ڵ��� �����ɴϴ�
		BigEndianUnicode = 1,
		// little-endian ����Ʈ ������ �����ڵ� ���Ŀ� ���� ���ڵ��� �����ɴϴ�.
		Unicode = 2,
		// UTF-7 ���Ŀ� ���� ���ڵ��� �����ɴϴ�.
		UTF7 = 3,
		// UTF-8 ���Ŀ� ���� ���ڵ��� �����ɴϴ�.
		UTF8 = 4,
		// ASCII ���Ŀ� ���� ���ڵ��� �����ɴϴ�.
		ASCII = 5
	}
}
