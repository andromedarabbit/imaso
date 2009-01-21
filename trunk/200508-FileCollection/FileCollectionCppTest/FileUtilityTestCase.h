#ifndef CPP_UNIT_KAISTIZEN_FILEUTILITY_H
#define CPP_UNIT_KAISTIZEN_FILEUTILITY_H

#include <cppunit/TestCase.h>  
#include <cppunit/extensions/HelperMacros.h> 

class FileUtilityTestCase : public CPPUNIT_NS::TestFixture
{
	CPPUNIT_TEST_SUITE( FileUtilityTestCase );
	CPPUNIT_TEST( makedirRootOnly );
	CPPUNIT_TEST( makedirAll );
	CPPUNIT_TEST( deldirAll );
	CPPUNIT_TEST_SUITE_END();

protected:
	

public:
	
protected:
	void makedirRootOnly();
	void makedirAll();
	void deldirAll();
};


#endif // #ifndef CPP_UNIT_KAISTIZEN_FILEUTILITY_H