#include "stdafx.h"
#include "FileCollectionTestCase.h"

#include <sys/types.h>
#include <sys/stat.h>
#include <direct.h>

using namespace kaistizen;
using namespace std;

CPPUNIT_TEST_SUITE_REGISTRATION( FileCollectionTestCase );

FileCollectionTestCase::FileCollectionTestCase()
{
	db_home = "database";
	data_dir = "data";
	log_dir = "log";
	tmp_dir = "tmp";
	db_filename = "test_database.db";
}

void FileCollectionTestCase::setUp()
{
}

void FileCollectionTestCase::tearDown()
{
	FileUtility::del_dir(db_home.c_str(),true);
}


void FileCollectionTestCase::open()
{
	int ret = -1;

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);
	
	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	collection.close();
}

void FileCollectionTestCase::join_environment()
{
	int ret = -1;

	FileCollection<char,char> collection_1(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);
	FileCollection<char,char> collection_2(true,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection_1.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	ret = collection_2.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	collection_2.close();
	collection_1.close();
}


void FileCollectionTestCase::remove_database()
{
	int ret = -1;
	struct stat sb;
	string total_db_path = db_home + "\\" + data_dir + "\\" + db_filename;

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	collection.close();
	
	// Check if a database file was created.
	ret = stat(total_db_path.c_str(), &sb);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	// Delete the database file
	ret= collection.remove_database();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	// Check if the database file was deleted.
	ret = stat(total_db_path.c_str(), &sb);
	CPPUNIT_NS::assertEquals(-1,ret,CPPUNIT_SOURCELINE(),"");
}


void FileCollectionTestCase::remove_environment()
{
	int ret = -1;
	struct stat sb;
	string env_file1 = db_home + "\\__db.001";

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	collection.close();
	
	// Check if a environment file was created.
	ret = stat(env_file1.c_str(), &sb);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	// Delete the environment files
	ret= collection.remove_environment();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	// Check if the database file was deleted.
	ret = stat(env_file1.c_str(), &sb);
	CPPUNIT_NS::assertEquals(-1,ret,CPPUNIT_SOURCELINE(),"");
}

void FileCollectionTestCase::without_a_transaction()
{
	int ret = -1;

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	char *key = "key_put_without_a_transaction";
	char *data = "data_put_without_a_transaction";

	// put
	ret = collection.put_entry(key,strlen(key)+1,data,strlen(data)+1);	
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error pushing an entry.");

	// get
	char data_for_getting[256];
	memset(data_for_getting,0,sizeof(data_for_getting));

	ret = collection.get_entry(key,strlen(key)+1,data_for_getting,sizeof(data_for_getting));
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error getting the entry.");
	ret = strcmp(data,data_for_getting);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Data not match");

	collection.close();
}

void FileCollectionTestCase::with_a_transaction()
{
	int ret = -1;

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	// With a transaction
	char *key = "key_put_with_a_transaction";
	char *data = "data_put_with_a_transaction";

	// begin transaction
	DbTxn* tid = collection.get_transaction();

	// put
	ret = collection.put_entry(key,strlen(key)+1,data,strlen(data)+1,tid);	
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error pushing an entry.");

	// abort transaction
	tid->abort();

	// get
	char data_for_getting[256];
	memset(data_for_getting,0,sizeof(data_for_getting));

	ret = collection.get_entry(key,strlen(key)+1,data_for_getting,sizeof(data_for_getting));
	CPPUNIT_NS::assertEquals(-30989,ret,CPPUNIT_SOURCELINE(),"Error getting the entry.");

	// begin transaction
	tid = collection.get_transaction();

	// put
	ret = collection.put_entry(key,strlen(key)+1,data,strlen(data)+1,tid);	
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error pushing an entry.");


	// commit transaction
	tid->commit(0);

	// get
	memset(data_for_getting,0,sizeof(data_for_getting));

	ret = collection.get_entry(key,strlen(key)+1,data_for_getting,sizeof(data_for_getting));
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error getting the entry.");
	ret = strcmp(data,data_for_getting);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Data not match");

	// close
	collection.close();
}




void FileCollectionTestCase::update_an_entry()
{
	int ret = -1;

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	char *key = "key_update";
	char *data = "data_update";
	char *replace_data = "data_updated";

	// put
	ret = collection.put_entry(key,strlen(key)+1,data,strlen(data)+1);	
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error pushing an entry.");

	// update
	ret = collection.put_entry(key,strlen(key)+1,replace_data,strlen(replace_data)+1);	
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error updating an entry.");

	// get
	char data_for_getting[256];
	memset(data_for_getting,0,sizeof(data_for_getting));

	ret = collection.get_entry(key,strlen(key)+1,data_for_getting,sizeof(data_for_getting));
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error getting the entry.");
	ret = strcmp(replace_data,data_for_getting);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Data not match");

	// close
	collection.close();
}


void FileCollectionTestCase::delete_an_entry()
{
	int ret = -1;

	FileCollection<char,char> collection(false,db_home,data_dir,log_dir,tmp_dir,db_filename,&std::cerr);

	ret = collection.open();
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"");

	char *key = "key_update";
	char *data = "data_update";

	// put
	ret = collection.put_entry(key,strlen(key)+1,data,strlen(data)+1);	
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error pushing an entry.");

	// delete
	ret = collection.del_entry(key,strlen(key)+1);
	CPPUNIT_NS::assertEquals(0,ret,CPPUNIT_SOURCELINE(),"Error deleting an entry.");

	// get
	char data_for_getting[256];
	memset(data_for_getting,0,sizeof(data_for_getting));

	ret = collection.get_entry(key,strlen(key)+1,data_for_getting,sizeof(data_for_getting));
	CPPUNIT_NS::assertEquals(-30989,ret,CPPUNIT_SOURCELINE(),"Error getting the entry.");
	
	// close
	collection.close();
}

