#if UNIT_TESTS

using System;
using System.IO;
using System.Text;

using NUnit.Framework;

using kaistizen.ApplicationBlocks.MemoryBasedPacket;

namespace kaistizen.ApplicationBlocks.MemoryPacket.Tests
{
	/// <summary>
	/// MemoryPacketParameterCollectionø° ¥Î«— ø‰æ‡ º≥∏Ì¿‘¥œ¥Ÿ.
	/// </summary>
	[TestFixture]
	public class MemoryPacketParameterCollectionFixture
	{
		/// <summary>
		/// Duplicate Parameter Name 
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Test001()
		{
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@DeptID";
			packet1.Value = "GoodCompany";
			packet1.Size = 5;

			MemoryPacketParameter packet2 = new MemoryPacketParameter();
			packet2.ParameterName = "@DeptID";
			packet2.Value = "BadCompany";
			packet2.Size = 5;

			MemoryPacketParameterCollection collection = new MemoryPacketParameterCollection();
			collection.Add(packet1);
			collection.Add(packet2);
		}

		/// <summary>
		/// Binary Formats
		/// </summary>
		[Test]
		public void Test002()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter. - Binary
			string source1 = "GoodCompany";
			byte[] dest1 = StringToByteArray(source1);

			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@ByteArray1";
			packet1.Size = dest1.Length;
			packet1.Value = dest1;
			packet1.MpType = MpType.Binary;
			packet1.FixedSize = false;

			// Create a Second Parameter. - Binary
			string source2 = "GoodCompany";
			byte[] dest2 = StringToByteArray(source2);
			int NullSize2 = 5;
			
			MemoryPacketParameter packet2 = new MemoryPacketParameter();
			packet2.ParameterName = "@ByteArray2";
			packet2.Size = dest2.Length + NullSize2;
			packet2.Value = dest2;
			packet2.MpType = MpType.Binary;
			packet2.FixedSize = false;

			// Create a Third Parameter. - Binary
			string source3 = "GoodCompany";
			byte[] dest3 = StringToByteArray(source3);
			int NullSize3 = 5;
			
			MemoryPacketParameter packet3 = new MemoryPacketParameter();
			packet3.ParameterName = "@ByteArray3";
			packet3.Size =  dest3.Length + NullSize3;
			packet3.Value = dest3;
			packet3.MpType = MpType.Binary;
			packet3.FixedSize = true;

			// Create a Fourth Parameter. - Binary
			string source4 = "GoodCompany";
			byte[] dest4 = StringToByteArray(source4);
			int removeSize4 = 5;
			
			MemoryPacketParameter packet4 = new MemoryPacketParameter();
			packet4.ParameterName = "@ByteArray4";
			packet4.Size = dest4.Length - removeSize4;
			packet4.Value = dest4;
			packet4.MpType = MpType.Binary;
			packet4.FixedSize = true;


			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
			// Add a Second Parameter to the Command
			cmd.Parameters.Add(packet2);
			// Add a Third Parameter to the Command
			cmd.Parameters.Add(packet3);
			// Add a Fourth Parameter to the Command
			cmd.Parameters.Add(packet4);
		

			// IgnoreOverflow?
			cmd.IgnoreOverflow = IgnoreOverflowMode.Ignore;

			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;

			int position = 0;

			// Validate..........
			// First 
			string outputValue1 = Encoding.Default.GetString(memoryPacket,0,packet1.Size);

			Assert.AreEqual(0,outputValue1.CompareTo(source1),"First Parameter");

			position = position + packet1.Size;

			// Second
			string outputValue2 = Encoding.Default.GetString(memoryPacket,position,packet2.Size - NullSize2);
		
			Assert.AreEqual(0,outputValue2.CompareTo(source2),"Second Parameter");

			if( memoryPacket[position + packet2.Size + 1] == 0 )
			{
				Assert.IsTrue(false,"Second Parameter");
			}
			
			position = position + packet2.Size - NullSize2;
			
			// Third
			string outputValue3 = Encoding.Default.GetString(memoryPacket,position,packet3.Size - NullSize3);
			
			Assert.AreEqual(0,outputValue3.CompareTo(source3),"Third Parameter");

			for(int i=position + packet3.Size - NullSize3; i<position + packet3.Size; i++)
			{
				Assert.AreEqual(0,memoryPacket[i],String.Format("{0} th byte's value is {1}",i,memoryPacket[i]));
			}

			position = position + packet3.Size;



			// Fourth
			string outputValue4 = Encoding.Default.GetString(memoryPacket,position,packet4.Size);
			
			Assert.AreEqual(0,outputValue4.CompareTo(source4.Substring(0,source4.Length-removeSize4)),"Fourth Parameter");

			position = position + packet4.Size;


			// Size
			Assert.AreEqual(totalBytes,position,"Position");

		}


		/// <summary>
		/// Binary Formats
		/// </summary>
		[Test]
		[ExpectedException(typeof(MemoryPacketException))]
		public void Test003()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter. - Binary
			string source1 = "GoodCompany";
			byte[] dest1 = StringToByteArray(source1);

			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@ByteArray1";
			packet1.Size = dest1.Length - 2;
			packet1.Value = dest1;
			packet1.MpType = MpType.Binary;
			packet1.FixedSize = false;

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);

			// IgnoreOverflow?
			cmd.IgnoreOverflow = IgnoreOverflowMode.Exception;


			// Create a Memory Packet Byte Array
			cmd.ToBytes();
		}

		/// Byte Data --> MemoryPacket
		[Test]
		public void Test004()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@Byte";
			// packet2.Size = 1;
			packet1.Value = Byte.MaxValue;
			packet1.MpType = MpType.Byte;
			// packet2.FixedSize = true;

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);

			
			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;


			int position = 0;

			// Validate..........
			// First
			byte outputValue1 = memoryPacket[position];
			Assert.AreEqual( outputValue1,(byte)packet1.Value,"Transformation of Byte Data is Failed.");
			position = position + packet1.Size;

			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}

		/// <summary>
		/// Int16 Data --> MemoryPacket
		/// </summary>
		[Test]
		public void Test005()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@Int16";
			// packet1.Size = 1;
			packet1.Value = Int16.MinValue;
			packet1.MpType = MpType.Int16;
			// packet1.FixedSize = true;

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
			
			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;


			int position = 0;

			// Validate..........
			// First
			MemoryStream ms1 = new MemoryStream(memoryPacket,position,packet1.Size,false);
			BinaryReader br1 = new BinaryReader(ms1);
			Int16 outputValue1 = br1.ReadInt16();
			br1.Close();

			Assert.AreEqual( outputValue1,(Int16)packet1.Value,"Transformation of Int16 Data is Failed.");
			
			position = position + packet1.Size;

			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}

		/// <summary>
		/// Int32 Data --> MemoryPacket
		/// </summary>
		[Test]
		public void Test006()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@Int32";
			// packet1.Size = 1;
			packet1.Value = Int32.MinValue;
			packet1.MpType = MpType.Int32;
			// packet1.FixedSize = true;


			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
		

			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;


			int position = 0;

			// Validate..........
			// First
			MemoryStream ms1 = new MemoryStream(memoryPacket,position,packet1.Size,false);
			BinaryReader br1 = new BinaryReader(ms1);
			Int32 outputValue1 = br1.ReadInt32();
			br1.Close();

			Assert.AreEqual( outputValue1,(Int32)packet1.Value,"Transformation of Int32 Data is Failed.");
			
			position = position + packet1.Size;

	
			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}


		/// <summary>
		/// Int64 Data --> MemoryPacket
		/// UInt64 Data --> MemoryPacket
		/// </summary>
		[Test]
		public void Test007()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@Int64";
			// packet1.Size = 1;
			packet1.Value = Int64.MinValue;
			packet1.MpType = MpType.Int64;
			// packet1.FixedSize = true;


			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);

		
			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;


			int position = 0;

			// Validate..........
			// First
			MemoryStream ms1 = new MemoryStream(memoryPacket,position,packet1.Size,false);
			BinaryReader br1 = new BinaryReader(ms1);
			Int64 outputValue1 = br1.ReadInt64();
			br1.Close();

			Assert.AreEqual( outputValue1,(Int64)packet1.Value,"Transformation of Int64 Data is Failed.");
			
			position = position + packet1.Size;


			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}


		/// <summary>
		/// String --> MemoryPacket
		/// </summary>
		[Test]
		public void Test008()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@string1";
			packet1.Size = 6;
			packet1.Value = "Ω¥∆€∏«";
			packet1.MpType = MpType.String;
			packet1.FixedSize = true;
			packet1.EncodingType = EncodingType.Default;

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
			
			
			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;

			int position = 0;

			// Validate..........
			// First
			MemoryStream ms1 = new MemoryStream(memoryPacket,position,packet1.Size,false);
			BinaryReader br1 = new BinaryReader(ms1);
			byte[] buf = br1.ReadBytes(packet1.Size);
			br1.Close();

			string msg = Encoding.Default.GetString(buf);
			Assert.AreEqual( msg,(string)packet1.Value,"Transformation of string Data is Failed.");
			
			position = position + packet1.Size;
			
			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}

		/// <summary>
		/// String --> MemoryPacket
		/// </summary>
		[Test]
		[ExpectedException(typeof(MemoryPacketException))]
		public void Test009()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@string1";
			packet1.Size = 4;
			packet1.Value = "Ω¥∆€∏«";
			packet1.MpType = MpType.String;
			packet1.FixedSize = true;
			packet1.EncodingType = EncodingType.Default;
			packet1.IgnoreOverflow = IgnoreOverflowMode.Exception; 

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
			
			
			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;

			int position = 0;

			// Validate..........
			// First
			MemoryStream ms1 = new MemoryStream(memoryPacket,position,packet1.Size,false);
			BinaryReader br1 = new BinaryReader(ms1);
			byte[] buf = br1.ReadBytes(packet1.Size);
			br1.Close();

			string msg = Encoding.Default.GetString(buf);
			Assert.AreEqual( msg,(string)packet1.Value,"Transformation of string Data is Failed.");
			
			position = position + packet1.Size;
			
			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}

		/// <summary>
		/// String --> MemoryPacket
		/// </summary>
		[Test]
		public void Test010()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter - Byte
			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@string1";
			packet1.Size = 4;
			packet1.Value = "Ω¥∆€∏«";
			packet1.MpType = MpType.String;
			packet1.FixedSize = true;
			packet1.EncodingType = EncodingType.Default;
			packet1.IgnoreOverflow = IgnoreOverflowMode.Ignore; 

			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
			
			
			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;

			int position = 0;

			// Validate..........
			// First
			MemoryStream ms1 = new MemoryStream(memoryPacket,position,packet1.Size,false);
			BinaryReader br1 = new BinaryReader(ms1);
			byte[] buf = br1.ReadBytes(packet1.Size);
			br1.Close();

			string msg = Encoding.Default.GetString(buf);
			Assert.AreEqual( msg,"Ω¥∆€","Transformation of string Data is Failed.");
			
			position = position + packet1.Size;
			
			// Size
			Assert.AreEqual(totalBytes,position,"Position");
		}


		/// <summary>
		/// Binary Data --> MemoryPacket
		/// Byte Data --> MemoryPacket
		/// Int16 --> MemoryPacket
		/// Int32 --> MemoryPacket
		/// Int64 ==> MemoryPacket
		/// </summary>
		[Test]
		public void Test011()
		{
			MemoryPacketCommand cmd = new MemoryPacketCommand();

			// Create a First Parameter. - Binary
			string source1 = "GoodCompany";
			byte[] dest1 = StringToByteArray(source1);

			MemoryPacketParameter packet1 = new MemoryPacketParameter();
			packet1.ParameterName = "@ByteArray";
			packet1.Size = dest1.Length;
            packet1.Value = dest1;
			packet1.MpType = MpType.Binary;
			packet1.FixedSize = false;
			packet1.IgnoreOverflow = IgnoreOverflowMode.Ignore;

			// Create a Second Parameter - Byte
			MemoryPacketParameter packet2 = new MemoryPacketParameter();
			packet2.ParameterName = "@Byte";
			// packet2.Size = 1;
			packet2.Value = (byte)120;
			packet2.MpType = MpType.Byte;
			// packet2.FixedSize = true;

			// Create a Third Parameter - Int16
			MemoryPacketParameter packet3 = new MemoryPacketParameter();
			packet3.ParameterName = "@Int16";
			// packet3.Size = 2;
			packet3.Value = Int16.MaxValue;
			packet3.MpType = MpType.Int16;
			// packet3.FixedSize = true;

			// Create a Fourth Parameter - Int32
			MemoryPacketParameter packet4 = new MemoryPacketParameter();
			packet4.ParameterName = "@Int32";
			// packet4.Size = 4;
			packet4.Value = Int32.MaxValue;
			packet4.MpType = MpType.Int32;
			// packet4.FixedSize = true;

			
			// Create a 5th Parameter - Int64
			MemoryPacketParameter packet5 = new MemoryPacketParameter();
			packet5.ParameterName = "@Int64";
			// packet4.Size = 4;
			packet5.Value = Int64.MaxValue;
			packet5.MpType = MpType.Int64;
			// packet4.FixedSize = true;

			
			// Add a First Parameter to the Command
			cmd.Parameters.Add(packet1);
			// Add a Second Parameter to the Command
			cmd.Parameters.Add(packet2);
			// Add a Third Parameter to the Command
			cmd.Parameters.Add(packet3);
			// Add a Fourth Parameter to the Command
			cmd.Parameters.Add(packet4);
			// Add a 5th Parameter to the Command
			cmd.Parameters.Add(packet5);


			// Create a Memory Packet Byte Array
			byte[] memoryPacket = null;
			
			memoryPacket = cmd.ToBytes();

			long totalBytes = memoryPacket.LongLength;


			int position = 0;

			// Validate..........
			// First 
			string outputValue1 = Encoding.Default.GetString(memoryPacket,0,packet1.Size);

			if( outputValue1.CompareTo(source1) != 0 )
			{
				Assert.IsTrue(false,"Transformation of Binary Data is Failed.");
			}

			position = position + packet1.Size;

			// Second
			byte outputValue2 = memoryPacket[position];
            Assert.IsTrue( (outputValue2 == (byte)packet2.Value),"Transformation of Byte Data is Failed.");
			position = position + packet2.Size;

			// Third
			MemoryStream ms3 = new MemoryStream(memoryPacket,position,packet3.Size,false);
			BinaryReader br3 = new BinaryReader(ms3);
			Int16 outputValue3 = br3.ReadInt16();
			br3.Close();

			Assert.IsTrue( ( outputValue3 == (Int16)packet3.Value),"Transformation of Int16 Data is Failed.");
			position = position + packet3.Size;


			// Fourth
			MemoryStream ms4 = new MemoryStream(memoryPacket,position,packet4.Size,false);
			BinaryReader br4 = new BinaryReader(ms4);
			Int32 outputValue4 = br4.ReadInt32();
			br4.Close();

			Assert.IsTrue( ( outputValue4 == (Int32)packet4.Value),"Transformation of Int32 Data is Failed.");
			position = position + packet4.Size;

			// 5th
			MemoryStream ms5 = new MemoryStream(memoryPacket,position,packet5.Size,false);
			BinaryReader br5 = new BinaryReader(ms5);
			Int64 outputValue5 = br5.ReadInt64();
			br5.Close();

			Assert.IsTrue( ( outputValue5 == (Int64)packet5.Value),"Transformation of Int64 Data is Failed.");
			position = position + packet5.Size;


			// Check bytes count
			Assert.AreEqual(totalBytes,position,"Position");
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