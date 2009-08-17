#pragma once


inline void EntryPoint()
{
	ScriptEngine::AssemblyLoader::Load(L"LogManagedTest");
	// ScriptEngine::Engine^ engine = gcnew ScriptEngine::Engine();
	// engine->Initialize();

	NativeToManagedTest::Call();
}