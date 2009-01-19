// TimeLibTest.cpp : �ܼ� ���� ���α׷��� ���� �������� �����մϴ�.
//

#include "stdafx.h"
#include <iostream>
#include <fstream>

#include "TestReporterStdout.h"
#include "XmlTestReporter.h"

using namespace UnitTest;


int RunTests(UnitTest::TestReporter& reporter)
{
	// return UnitTest::RunAllTests(reporter, UnitTest::Test::GetTestList(), NULL, 0);
	TestRunner runner(reporter);
	return runner.RunTestsIf(Test::GetTestList(), NULL, True(), 0);
}

int _tmain(int argc, _TCHAR* argv[])
{
	// ����ٿ��� ���� �̸��� �Ѱ����� �׽�Ʈ ����� XML �������� ���Ͽ� ����Ѵ�.
	std::vector<std::wstring> args;
	if(argv)
	{
		while(*argv != NULL)
		{
			args.push_back(*argv);
			argv ++;
		}
	}

	// ����� ǥ������¿� ��½�Ų��.
	if(args.size() == 1)
	{
		UnitTest::TestReporterStdout reporter;
		return RunTests(reporter);
	}

	// ����� �ؽ�Ʈ ���Ͽ� XML ������� ��½�Ų��.
	std::ofstream f(args[1].c_str(), std::ios_base::ate);
	UnitTest::XmlTestReporter reporter(f);
	return RunTests(reporter);
}
