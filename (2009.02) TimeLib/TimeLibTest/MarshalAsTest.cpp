#include "stdafx_clr.h"
#include "MarshalAs.h"

using namespace System;
using namespace msclr::interop;

SUITE(MarshalAs)
{
	TEST(NativeToManaged)
	{
		NTimeSpan nativeOneDay = NTimeSpan::TicksPerDay;
		TimeSpan managedOneDay = marshal_as<TimeSpan>(nativeOneDay);
		
		CHECK_EQUAL(1, managedOneDay.Days);		
	}

	TEST(ManagedToNative)
	{
		TimeSpan managedOneDay = TimeSpan::FromDays(1);
		NTimeSpan nativeOneDay = marshal_as(managedOneDay);
	
		CHECK_EQUAL(1, nativeOneDay.Days());		
	}

	TEST(ManagedToNativeByUsingContext)
	{
		TimeSpan managedOneDay = TimeSpan::FromDays(1);

		marshal_context context;
		NTimeSpan* nativeOneDayPtr = context.marshal_as<NTimeSpan*>(managedOneDay);
	
		CHECK_EQUAL(1, nativeOneDayPtr->Days());		
	}
}
