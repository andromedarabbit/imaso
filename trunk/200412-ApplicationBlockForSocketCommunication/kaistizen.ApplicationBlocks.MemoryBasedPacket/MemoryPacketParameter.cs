using System;

namespace kaistizen.ApplicationBlocks.MemoryBasedPacket
{

	/// <summary>
	/// MemoryPacketParameter에 대한 요약 설명입니다.
	/// </summary>
	public class MemoryPacketParameter : IComparable
	{
		private int size;
        private string parameter_name;
        private object parameter_value;
        private MpType mp_type;
        private EncodingType encoding_type;
        private bool fixed_size;
        private IgnoreOverflowMode ignoreOverflow;
        private bool network_byte_order;

	
		public MemoryPacketParameter()
			: this(String.Empty,MpType.Binary,null,true,0,EncodingType.Default,true,IgnoreOverflowMode.Neutral)
		{
		}

		public MemoryPacketParameter(string parameterName,MpType mpType,object value,bool fixedSize,int size,EncodingType encodingType,bool networkByteOrder,IgnoreOverflowMode ignoreOverflow)
		{
			ParameterName = parameterName;
			MpType = mpType;
			Value = value;
			FixedSize = fixedSize;
			Size = size;
			EncodingType = encodingType;
			NetworkByteOrder = networkByteOrder;
			IgnoreOverflow = ignoreOverflow;
		}
		
		public MemoryPacketParameter(MemoryPacketParameter parm) 
			: this(parm.ParameterName,parm.MpType,parm.Value,parm.FixedSize,parm.Size,parm.EncodingType,parm.NetworkByteOrder,parm.IgnoreOverflow)
		{
		}

		public static MemoryPacketParameter NewInstance(MemoryPacketParameter parm)
		{
			return new MemoryPacketParameter(parm);
		}

		public virtual string ParameterName
		{
			get { return parameter_name; }
			set { parameter_name = value; } 
		}

		/// <summary>
		/// 열 내부에 있는 데이터의 최대 크기를 바이트 단위로 가져오거나 설정합니다.
		/// </summary>
		public virtual int Size 
		{
			get { return size; } 
			set { size = value; } 
		}
		
		public virtual object Value
		{
			get { return parameter_value; } 
			set { parameter_value = value; } 
		}

		public virtual MpType MpType
		{
			get { return mp_type; } 
			set { mp_type = value; }
		}

		public virtual EncodingType EncodingType
		{
			get { return encoding_type; } 
			set { encoding_type = value; }
		}

		public virtual bool FixedSize
		{
			get { return fixed_size; }
			set { fixed_size = value; }
		}

		public virtual IgnoreOverflowMode IgnoreOverflow
		{
			get { return ignoreOverflow; }
			set { ignoreOverflow = value; }
		}

		public virtual bool NetworkByteOrder
		{
			get { return network_byte_order; }
			set { network_byte_order = value; }
		}

		public override bool Equals(object obj)
		{
			if( this == obj )  
			{
				return true;
			}

			if( (obj is MemoryPacketParameter) == false )
			{
				return false;
			}

			MemoryPacketParameter parm = (MemoryPacketParameter)obj;
			
			return parm.ParameterName.Equals(ParameterName);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public int CompareTo(object obj)
		{
			if( this == obj )
			{
				return 0;
			}

			MemoryPacketParameter parm = (MemoryPacketParameter)obj;

			return parm.ParameterName.CompareTo(ParameterName);			
		}
	}
}
