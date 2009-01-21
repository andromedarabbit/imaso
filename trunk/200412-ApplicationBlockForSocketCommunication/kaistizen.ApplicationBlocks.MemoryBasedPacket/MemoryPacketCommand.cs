using System;
using System.IO;
using System.Text;
using System.Net;

namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{
	/// <summary>
	/// MemoryPacketCommand에 대한 요약 설명입니다.
	/// </summary>
	public class MemoryPacketCommand
	{
        private MemoryPacketParameterCollection parameters;
        private IgnoreOverflowMode ignoreOverflow;

		public MemoryPacketCommand() : this(IgnoreOverflowMode.Neutral)
		{
		}

		public MemoryPacketCommand(IgnoreOverflowMode mode)
		{
			IgnoreOverflow = mode;
			parameters = new MemoryPacketParameterCollection();
		}

		public virtual MemoryPacketParameterCollection FromBytes(byte[] bytes)
		{            
			MemoryPacketParameterCollection newCollection = new MemoryPacketParameterCollection(parameters);

			using( MemoryStream ms = new MemoryStream(bytes) ) 
			{
				using( BinaryReader br = new BinaryReader(ms) )
				{
					foreach(MemoryPacketParameter parm in newCollection)
					{
						MemoryStreamToParameter(parm, br);
					}
				}
			}

			return newCollection;
		}

		private void MemoryStreamToParameter(MemoryPacketParameter parm, BinaryReader br)
		{
			switch( parm.MpType )
			{
				case MpType.Binary:
					GetBinary(parm, br);
					break;
				case MpType.Byte:
					GetByte(parm,br);
					break;
				case MpType.Int16:
					GetInt16(parm, br);
					break;
				case MpType.Int32:
					GetInt32(parm, br);
					break;
				case MpType.Int64:
					GetInt64(parm, br);
					break;
				case MpType.String:
					GetString(parm, br);
					break;
				default:
					string message = GetInvalidMemoryPacketTypeExceptionMsg(parm);
					throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidPacketType);
			}
		}

		private static string GetInvalidMemoryPacketTypeExceptionMsg(MemoryPacketParameter parm)
		{
			return String.Format("Invalid Memory-Based Packet Type '{0}'.",parm.MpType.ToString());
		}

		private static void GetInt64(MemoryPacketParameter parm, BinaryReader br)
		{
			if( parm.NetworkByteOrder == true )
			{
				parm.Value = IPAddress.NetworkToHostOrder( br.ReadInt64() );
			}
			else
			{
				parm.Value = br.ReadInt64();
			}
		}

		private static void GetInt32(MemoryPacketParameter parm, BinaryReader br)
		{
			if( parm.NetworkByteOrder == true )
			{
				parm.Value = IPAddress.NetworkToHostOrder( br.ReadInt32() );
			}
			else
			{
				parm.Value = br.ReadInt32();
			}
		}

		private static void GetInt16(MemoryPacketParameter parm, BinaryReader br)
		{
			if( parm.NetworkByteOrder == true )
			{
				parm.Value = IPAddress.NetworkToHostOrder( br.ReadInt16() );
			}
			else
			{
				parm.Value = br.ReadInt16();
			}
		}

		private static void GetByte(MemoryPacketParameter parm, BinaryReader br)
		{
			parm.Value = br.ReadByte();
		}

		private static void GetBinary(MemoryPacketParameter parm, BinaryReader br)
		{
			if( parm.FixedSize == true )
			{
				parm.Value = br.ReadBytes(parm.Size);
			}
			else
			{
				string message = GetFixedSizeExceptionMsg(parm.ParameterName);
				throw new MemoryPacketException(message,MemoryPacketErrorCode.FixedSize);
			}
		}

		private static string GetFixedSizeExceptionMsg(string parameterName)
		{
			return String.Format("Property 'FixedSize' of the parameter '{0}' must be set 'true'.",parameterName);
		}

		private static void GetString(MemoryPacketParameter parm, BinaryReader br)
		{
			if( parm.FixedSize == true )
			{
				byte[] bytes = br.ReadBytes(parm.Size);

				switch( parm.EncodingType )
				{
					case EncodingType.ASCII:
						parm.Value = Encoding.ASCII.GetString(bytes);
						break;
					case EncodingType.Default:
						parm.Value = Encoding.Default.GetString(bytes);
						break;
					case EncodingType.BigEndianUnicode:
						parm.Value = Encoding.BigEndianUnicode.GetString(bytes);
						break;
					case EncodingType.Unicode:
						parm.Value = Encoding.Unicode.GetString(bytes);
						break;
					case EncodingType.UTF7:
						parm.Value = Encoding.UTF7.GetString(bytes);
						break;
					case EncodingType.UTF8:
						parm.Value = Encoding.UTF8.GetString(bytes);
						break;
					default:
						string message = String.Format("Invalid Encoding Type '{0}'.",parm.MpType.ToString());
						throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidEncodingType);
				}
			}
			else
			{
				string message = GetFixedSizeExceptionMsg(parm.ParameterName);
				throw new MemoryPacketException(message,MemoryPacketErrorCode.FixedSize);
			}
		}


		public virtual byte[] ToBytes()
		{
            byte[] dest = null;

			using( MemoryStream ms = new MemoryStream() )
			{
				foreach(MemoryPacketParameter parm in parameters)
				{
					ParameterToMemoryStream(parm, ms);
				}

				dest = ms.ToArray();
			}

			return dest;;
		}

		private void ParameterToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			switch( parm.MpType )
			{
				case MpType.Binary:
					BinaryToMemoryStream(parm, ms);
					break;
				case MpType.Byte:
					ByteToMemoryStream(parm, ms);
					break;
				case MpType.Int16:
					Int16ToMemoryStream(parm, ms);
					break;
				case MpType.Int32:
					Int32ToMemoryStream(parm, ms);
					break;
				case MpType.Int64:
					Int64ToMemoryStream(parm, ms);
					break;
				case MpType.String:
					StringToMemoryStream(parm, ms);
					break;
				default:
					string message = GetInvalidMemoryPacketTypeExceptionMsg(parm);
					throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidPacketType);
			}
		}


		public virtual IgnoreOverflowMode IgnoreOverflow
		{
			get { return ignoreOverflow; } 
			set { ignoreOverflow = value; }
		}

		public MemoryPacketParameterCollection Parameters
		{
			get { return parameters; }
		}

		private void StringToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			if( parm.Value.GetType() != typeof(string) )
			{
				string message = GetInvalidParameterExceptionMessage(parm.ParameterName,typeof(string),parm.Value.GetType());
				throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidParameterValue);
			}
	
			string parm_value = (string)parm.Value;
	
			byte[] buf = GetBytes(parm, parm_value);
	
			if( parm.Size >= buf.Length )
			{
				ms.Write(buf,0,buf.Length);

				if( parm.FixedSize == true )
				{
					for(int i=0; i<parm.Size-buf.Length; i++)
					{
						ms.WriteByte(0);
					}
				}
			}
			else
			{
				if( CheckOverflowPolicy(parm) == true )
				{
					ms.Write(buf,0,parm.Size);
				}
				else
				{
					string message = GetOverflowExceptionMsg(parm);
					throw new MemoryPacketException(message,MemoryPacketErrorCode.Overflow);
				}
			}
		}

		private static string GetOverflowExceptionMsg(MemoryPacketParameter parm)
		{
			return String.Format("Overflow exception in transforming the value of parameter '{0}'.",parm.ParameterName);
		}

		private bool CheckOverflowPolicy(MemoryPacketParameter parm)
		{
			bool allowOverflow = false;

			if( IgnoreOverflow == IgnoreOverflowMode.Exception )
			{
				allowOverflow = false;	
			}
			else if( IgnoreOverflow == IgnoreOverflowMode.Neutral )
			{
				if( parm.IgnoreOverflow == IgnoreOverflowMode.Exception )
				{
					allowOverflow = false;
				}
				else if( parm.IgnoreOverflow == IgnoreOverflowMode.Neutral ) 
				{
					allowOverflow = false;
				}
				else // ( parm.IgnoreOverflow == IgnoreOverflowMode.Ignore )
				{
					allowOverflow = true;
				}
			}
			else // ( IgnoreOverflow == IgnoreOverflowMode.Ignore )
			{
				allowOverflow = true;				
			}

			return allowOverflow;
		}


		private static byte[] GetBytes(MemoryPacketParameter parm, string parm_value)
		{
			byte[] buf = null;

			switch( parm.EncodingType ) 
			{
				case EncodingType.ASCII:
					buf = Encoding.ASCII.GetBytes(parm_value);
					break;
				case EncodingType.BigEndianUnicode:
					buf = Encoding.BigEndianUnicode.GetBytes(parm_value);
					break;
				case EncodingType.Default:
					buf = Encoding.Default.GetBytes(parm_value);
					break;
				case EncodingType.Unicode:
					buf = Encoding.Unicode.GetBytes(parm_value);
					break;
				case EncodingType.UTF7:
					buf = Encoding.UTF7.GetBytes(parm_value);
					break;
				case EncodingType.UTF8:
					buf = Encoding.UTF8.GetBytes(parm_value);
					break;
				default:
					string message = String.Format("Invalid Encoding Type '{0}'.",parm.MpType.ToString());
					throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidEncodingType);
				
			}

			return buf;
		}


		private static void Int64ToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			if( parm.Value.GetType() != typeof(Int64) )
			{
				string message = GetInvalidParameterExceptionMessage(parm.ParameterName,typeof(Int64),parm.Value.GetType());
				throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidParameterValue);
			}
	
			parm.Size = 8;
			Int64 parm_value = (Int64)parm.Value;
	
			BinaryWriter bw = new BinaryWriter(ms,Encoding.Default);
			bw.Write(parm_value);
		}

	
		private static void Int32ToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			if( parm.Value.GetType() != typeof(Int32) )
			{
				string message = GetInvalidParameterExceptionMessage(parm.ParameterName,typeof(Int32),parm.Value.GetType());
				throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidParameterValue);
			}
	
			parm.Size = 4;
			Int32 parm_value = (Int32)parm.Value;
	
			BinaryWriter bw = new BinaryWriter(ms,Encoding.Default);
			bw.Write(parm_value);
		}

		
		private static void Int16ToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			if( parm.Value.GetType() != typeof(Int16) )
			{
				string message = GetInvalidParameterExceptionMessage(parm.ParameterName,typeof(Int16),parm.Value.GetType());
				throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidParameterValue);
			}
	
			parm.Size = 2;
			short parm_value = (Int16)parm.Value;
	
			BinaryWriter bw = new BinaryWriter(ms,Encoding.Default);
			bw.Write(parm_value);
		}


		private static void ByteToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			if( parm.Value.GetType() != typeof(byte) )
			{
				string message = GetInvalidParameterExceptionMessage(parm.ParameterName,typeof(byte),parm.Value.GetType());
				throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidParameterValue);
			}
	
			parm.Size = 1;

			ms.WriteByte((byte)parm.Value);
		}

		private void BinaryToMemoryStream(MemoryPacketParameter parm, MemoryStream ms)
		{
			if( parm.Value.GetType() != typeof(byte[]) )
			{
				string message = GetInvalidParameterExceptionMessage(parm.ParameterName,typeof(byte[]),parm.Value.GetType());
				throw new MemoryPacketException(message,MemoryPacketErrorCode.InvalidParameterValue);
			}
	
			byte[] parm_value = (byte[])parm.Value;

			if( parm.Size >= parm_value.Length )
			{
				ms.Write(parm_value,0,parm_value.Length);	
					
				if( parm.FixedSize == true )
				{
					for(int i=0; i<parm.Size-parm_value.Length; i++)
					{
						ms.WriteByte((byte)0);
					}
				}
			}
			else
			{
				if( CheckOverflowPolicy(parm) == true )
				{
					ms.Write(parm_value,0,parm.Size);
				}
				else
				{
					string message = GetOverflowExceptionMsg(parm);
					throw new MemoryPacketException(message,MemoryPacketErrorCode.Overflow);
				}
			}
		}

		private static string GetInvalidParameterExceptionMessage(string parameterName,Type mustBe,Type current)
		{
			return String.Format("The type of a parameter '{0}' must be '{1}', not '{2}'.",parameterName,mustBe.ToString(),current.ToString());
		}

	}
}
