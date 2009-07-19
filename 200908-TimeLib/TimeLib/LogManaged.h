#pragma once
#include <msclr/marshal_cppstd.h>

#include "Log.h"

public ref class LogManaged abstract sealed
{
public:
	static void Write(System::String^ msg)
	{
		// std::wcout.imbue(std::locale("korean"));
		std::wstring nativeMsg = msclr::interop::marshal_as<std::wstring>(msg);
		Log::Write(nativeMsg);
	}
};
