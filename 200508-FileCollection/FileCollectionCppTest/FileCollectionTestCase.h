#ifndef CPP_UNIT_KAISTIZEN_FILECOLLECTION_H__
#define CPP_UNIT_KAISTIZEN_FILECOLLECTION_H__

#include <cppunit/TestCase.h>  
#include <cppunit/extensions/HelperMacros.h> 

#include <string>
#include "FileCollection.hpp"

class FileCollectionTestCase : public CPPUNIT_NS::TestFixture
{
	CPPUNIT_TEST_SUITE( FileCollectionTestCase );
	CPPUNIT_TEST( open );
	CPPUNIT_TEST( join_environment );
	CPPUNIT_TEST( remove_database );
	CPPUNIT_TEST( remove_environment );
	CPPUNIT_TEST( without_a_transaction );
	CPPUNIT_TEST( with_a_transaction );
	CPPUNIT_TEST( update_an_entry );
	CPPUNIT_TEST( delete_an_entry );
	CPPUNIT_TEST_SUITE_END();

public:
	void setUp();
	void tearDown();

	FileCollectionTestCase();
protected:
	void open();
	void join_environment();
	void remove_database();
	void remove_environment();
	void without_a_transaction();
	void with_a_transaction();
	void update_an_entry();
	void delete_an_entry();

	std::string db_home;
	std::string data_dir;
	std::string log_dir;
	std::string tmp_dir;
	std::string db_filename;
};




#endif 
