// TimeLibTest.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
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
	// 명령줄에서 파일 이름을 넘겼으면 테스트 결과를 XML 형식으로 파일에 기록한다.
	std::vector<std::wstring> args;
	if(argv)
	{
		while(*argv != NULL)
		{
			args.push_back(*argv);
			argv ++;
		}
	}

	// 결과를 표준입출력에 출력시킨다.
	if(args.size() == 1)
	{
		UnitTest::TestReporterStdout reporter;
		return RunTests(reporter);
	}

	// 결과를 텍스트 파일에 XML 양식으로 출력시킨다.
	std::ofstream f(args[1].c_str(), std::ios_base::ate);
	UnitTest::XmlTestReporter reporter(f);
	return RunTests(reporter);
}
