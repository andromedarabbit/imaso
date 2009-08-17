#include "stdafx.h"
#include "TimeSpanTest.h"

#include "TimeSpan.h"

SUITE(TimeSpan)
{
	TEST(ImplicitConstruction)
	{
		NTimeSpan oneday1 = NTimeSpan(NTimeSpan::TicksPerDay);
		NTimeSpan oneday2 = NTimeSpan::TicksPerDay;

		CHECK_EQUAL(oneday1.Ticks(), oneday2.Ticks());
	}

    TEST(ComparisonOperators)
    {
		NTimeSpan timestamp1 = NTimeSpan::TicksPerDay;
		NTimeSpan timestamp2 = NTimeSpan::TicksPerMinute;
		
		CHECK(timestamp1 > timestamp2);
		CHECK(timestamp1 >= timestamp2);
		CHECK(timestamp2 < timestamp1);
		CHECK(timestamp2 <= timestamp1);
		CHECK( (timestamp2 == timestamp1) == false );
		CHECK( (timestamp2 != timestamp1) == true );
    }

	TEST(Calculations)
    {
		NTimeSpan oneDay = NTimeSpan::TicksPerDay;
		NTimeSpan oneHour = NTimeSpan::TicksPerHour;
		
		NTimeSpan hours23 = NTimeSpan::TicksPerHour * 23;
		CHECK_EQUAL(hours23, oneDay - oneHour);
		
		NTimeSpan days2 = NTimeSpan::TicksPerDay * 2;
		CHECK_EQUAL(days2, oneDay + oneDay);
    }


}