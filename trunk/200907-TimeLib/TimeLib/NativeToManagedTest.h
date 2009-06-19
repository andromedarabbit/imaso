#pragma once

public ref class NativeToManagedTest abstract sealed
{
public:
	static void Call()
	{
		ScriptEngine::NativeToManagedCaller::Call(L"LogManagedTest.NativeToManaged", L"CallMe");
	}
};

