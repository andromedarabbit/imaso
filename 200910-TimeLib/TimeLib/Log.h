#pragma once

class Log
{
private:
	explicit Log();
	explicit Log(const Log&);
	Log& operator = (const Log&);

public:
	static void Write(const std::wstring& msg)
	{
		std::wcout << msg << std::endl;
	}

};