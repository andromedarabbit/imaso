#pragma once

typedef signed __int64 TIMESPANTICKS;


class NTimeSpan
{
	friend class NDateTime;
public:
	static const TIMESPANTICKS TicksPerDay =  864000000000; // 24 * 60 * 60 * 1000 * 1000 * 10
	static const TIMESPANTICKS TicksPerHour = 36000000000; // 60 * 60 * 1000 * 1000 * 10
	static const TIMESPANTICKS TicksPerMinute = 600000000; 
	static const TIMESPANTICKS TicksPerSecond = 10000000;
	static const TIMESPANTICKS TicksPerMillisecond = 10000;

	NTimeSpan()
		: m_Time(0)
	{
	}

	NTimeSpan(const TIMESPANTICKS ticks)
		: m_Time(ticks)
	{
	}

	NTimeSpan(const NTimeSpan& other)
		: m_Time(other.m_Time)
	{
	}

	inline TIMESPANTICKS Ticks() const
	{
		return m_Time;
	}

	inline int Days() const
	{
		return static_cast<int> (m_Time / TicksPerDay);
	}

	inline int Hours() const
	{
		return static_cast<int> ((m_Time / TicksPerHour) % 24L);
	}

	inline int Minutes() const
	{
		return static_cast<int> ((m_Time / TicksPerMinute) % 60L);
	}

	inline int Seconds() const
	{
		return static_cast<int> ((m_Time / TicksPerSecond) % 60L);
	}

	inline int Milliseconds() const
	{
		return static_cast<int> ((m_Time / TicksPerMillisecond) % 1000L);;
	}

	inline double TotalDays() const
	{
		return (m_Time * 1.15740740740741E-12);
	}

	inline double TotalHours() const
	{
		return (m_Time * 2.77777777777778E-11);
	}

	inline double TotalMinutes() const
	{
		return (m_Time * 1.66666666666667E-09);
	}

	inline double TotalSeconds() const
	{
		return (m_Time * 1E-07);
	}

	inline double TotalMilliseconds() const
	{
		double num = m_Time * 0.0001;
		if (num > 922337203685477)
		{
			return 922337203685477;
		}
		if (num < -922337203685477)
		{
			return -922337203685477;
		}
		return num;
	}

	NTimeSpan operator+(const NTimeSpan& other)
	{
		return NTimeSpan(this->m_Time + other.m_Time);
	}

	NTimeSpan operator+(const NTimeSpan& other) const
	{
		return NTimeSpan(this->m_Time + other.m_Time);
	}

	NTimeSpan operator-(const NTimeSpan& other)
	{
		return NTimeSpan(this->m_Time - other.m_Time);
	}

	NTimeSpan operator-(const NTimeSpan& other) const
	{
		return NTimeSpan(this->m_Time - other.m_Time);
	}

	bool operator==(const NTimeSpan& other) const
	{
		return this->m_Time == other.m_Time;
	}

	bool operator==(const TIMESPANTICKS otherTick) const
	{
		return this->m_Time == otherTick;
	}

	bool operator!=(const NTimeSpan& other) const
	{
		return this->m_Time != other.m_Time;
	}

	bool operator!=(const TIMESPANTICKS otherTick) const
	{
		return this->m_Time != otherTick;
	}

	bool operator<=(const NTimeSpan& other) const
	{
		return this->m_Time <= other.m_Time;	
	}

	bool operator<=(const TIMESPANTICKS otherTick) const
	{
		return this->m_Time <= otherTick;	
	}

	bool operator>=(const NTimeSpan& other) const
	{
		return this->m_Time >= other.m_Time;
	}

	bool operator<(const NTimeSpan& other) const
	{
		return this->m_Time < other.m_Time;
	}

	bool operator>(const NTimeSpan& other) const
	{
		return this->m_Time > other.m_Time;
	}

	bool operator<(const TIMESPANTICKS otherTick) const
	{
		return this->m_Time < otherTick;
	}

	bool operator>(const TIMESPANTICKS otherTick) const
	{
		return this->m_Time > otherTick;
	}

	operator TIMESPANTICKS() const
	{
		return m_Time;
	}

private:
	TIMESPANTICKS m_Time;
};
