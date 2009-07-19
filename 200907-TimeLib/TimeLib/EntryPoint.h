#pragma once


inline void EntryPoint()
{
	ScriptEngine::AssemblyLoader::Load(L"LogManagedTest");

	NativeToManagedTest::Call();
}