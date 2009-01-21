#if UNIT_TESTS

using NUnit.Framework;

using kaistizen.ApplicationBlocks.MemoryBasedPacket;

namespace kaistizen.ApplicationBlocks.MemoryPacket.Tests
{
	[TestFixture]	
	public class MemoryPacketParameterFixture
	{
		[Test]
		public void MethodEquals()
		{
			MemoryPacketParameter parm1 = new MemoryPacketParameter();
            parm1.ParameterName = "@DeptID";
			parm1.Value = "GoodCompany";
			parm1.Size = 15;
			parm1.FixedSize = true;
			parm1.MpType = MpType.String;
			parm1.EncodingType = EncodingType.Default;

			MemoryPacketParameter parm2 = new MemoryPacketParameter();
			parm2.ParameterName = "@DeptID";
			parm2.Value = "BadCompany";
			parm2.Size = 15;
			parm2.FixedSize = true;
			parm2.MpType = MpType.String;
			parm2.EncodingType = EncodingType.Default;

			Assert.AreEqual(true,parm1.Equals(parm1));
			Assert.AreEqual(false,parm1.Equals(null));
			Assert.AreEqual(true,parm1.Equals(parm2));
			Assert.AreEqual(true,parm2.Equals(parm1));
		}
	}
}

#endif