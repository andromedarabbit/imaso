#include "stdafx.h"
#include "FileUtilityTestCase.h"
#include "FileUtility.h"

#include <string>
#include <sys/types.h>
#include <sys/stat.h>
#include <direct.h>

using namespace kaistizen;
using namespace std;

CPPUNIT_TEST_SUITE_REGISTRATION( FileUtilityTestCase );

void FileUtilityTestCase::makedirRootOnly()
{
	struct stat sb;
	string dir = "root_directory";

	FileUtility::make_dir(dir.c_str());

	
	int ret = stat(dir.c_str(), &sb);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	rmdir(dir.c_str());
}

void FileUtilityTestCase::makedirAll()
{
	struct stat sb;
	string root_dir = "root_directory";
	string sub_dir_1 = root_dir + "\\depth1";
	string sub_dir_2 = sub_dir_1 + "\\depth2";

	FileUtility::make_dir(sub_dir_2.c_str());

	int ret = stat(sub_dir_2.c_str(), &sb);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	rmdir(sub_dir_2.c_str());
	rmdir(sub_dir_1.c_str());
	rmdir(root_dir.c_str());
}

void FileUtilityTestCase::deldirAll()
{
	int ret = -1;
	struct stat sb;

	string root_dir = "root_directory";
	string sub_dir_1 = root_dir + "\\depth1";
	string sub_dir_2 = sub_dir_1 + "\\depth2";

	mkdir(root_dir.c_str());
	mkdir(sub_dir_1.c_str());
	mkdir(sub_dir_2.c_str());

	ret = stat(sub_dir_2.c_str(), &sb);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"making directories");

	FileUtility::del_dir(root_dir.c_str(), false);
	ret = stat(sub_dir_2.c_str(), &sb);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"directories should exist");

	FileUtility::del_dir(root_dir.c_str(), true);
	ret = stat(sub_dir_2.c_str(), &sb) 
		+ stat(sub_dir_1.c_str(), &sb)
		+ stat(root_dir.c_str(), &sb);
	CPPUNIT_NS::assertEquals(-3,ret,CPPUNIT_SOURCELINE(),"directories should be deleted");
}
