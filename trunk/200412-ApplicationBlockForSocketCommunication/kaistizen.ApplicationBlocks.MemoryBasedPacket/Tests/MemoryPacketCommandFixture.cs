#if UNIT_TESTS

using System.Text;
using System.IO;

using NUnit.Framework;

using kaistizen.ApplicationBlocks.MemoryBasedPacket;

namespace kaistizen.ApplicationBlocks.MemoryPacket.Tests
{
	/// <summary>
	/// MemoryPacketCommand에 대한 요약 설명입니다.
	/// </summary>
	[TestFixture]	
	public class MemoryPacketCommandFixture
	{
		[Test]
		public void Methods()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter. - Binary
			string sourceString1 = "GoodCompany";
			byte[] source1 = StringToByteArray(sourceString1);

			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@ByteArray1";
			packet1.Size = source1.Length;
			packet1.MpType = MpType.Binary;
			packet1.FixedSize = true;

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
		
			MemoryPacketParameterCollection collection = cmd.FromBytes(source1);

			// byte[] dest1 = (byte[])packet1.Value;
			MemoryPacketParameter newParm1 = (MemoryPacketParameter)collection["@ByteArray1"];
            byte[] dest1 = (byte[])newParm1.Value;

			string outputValue1 = Encoding.Default.GetString(dest1,0,dest1.Length);

			Assert.AreEqual(0,outputValue1.CompareTo(sourceString1),"First Parameter");
		}

		[Test]
		[ExpectedException(typeof(MemoryPacketException))]
		public void ThrowMemoryPacketException()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter. - Binary
			string sourceString1 = "GoodCompany";
			byte[] source1 = StringToByteArray(sourceString1);

			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@ByteArray1";
			packet1.Size = source1.Length;
			packet1.MpType = MpType.Binary;
			packet1.FixedSize = false;

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
		
			cmd.FromBytes(source1);
		}

		
		private byte[] StringToByteArray(string source)
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms,Encoding.Default);

			bw.Write(source.ToCharArray());

			byte[] dest = ms.ToArray();

			ms.Close();

			return dest;
		}

	}
}

#endif