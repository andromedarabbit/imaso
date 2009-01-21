#ifndef FILE_COLLECTION_H__
#define FILE_COLLECTION_H__

#include <db_cxx.h>
#include <string>
#include <iostream>

namespace kaistizen
{
	#define DEFAULT_PROGRAM_NAME "FileCollection"

	const u_int32_t environment_open_flags_for_init = DB_CREATE | DB_INIT_LOCK | DB_INIT_LOG | DB_INIT_MPOOL | DB_INIT_TXN | DB_RECOVER | DB_THREAD;
	const u_int32_t environment_open_flags_for_join = DB_JOINENV | DB_THREAD;
	const u_int32_t database_open_flags = DB_CREATE | DB_AUTO_COMMIT | DB_THREAD;

	template <class KeyType,class DataType>
	class FileCollection
	{
	public:
		FileCollection(bool join,std::string &file);
		FileCollection(bool join,std::string &file,std::ostream* err_stream);
		FileCollection(bool join,std::string db_home,std::string data_dir,
			std::string log_dir,std::string tmp_dir,std::string &file,
			std::ostream* err_stream);
		                                                                                                                                
		~FileCollection() { }
		                                    
		inline DbEnv* getDbEnv() { return dbenv; }
		inline Db* getDb() { return db; }
		
		int open();
			                                                                                                                            
		int put_entry(const KeyType *key,u_int32_t key_size,const DataType *data,u_int32_t data_size);
		int put_entry(const KeyType *key,u_int32_t key_size,const DataType *data,u_int32_t data_size,DbTxn *tid);
		int get_entry(const KeyType *key,u_int32_t key_size,DataType *data,u_int32_t data_length);
		int get_entry(const KeyType *key,u_int32_t key_size,DataType *data,u_int32_t data_length,DbTxn *tid);
		int del_entry(const KeyType *key,u_int32_t key_size);
		int del_entry(const KeyType *key,u_int32_t key_size,DbTxn *tid);
		
		DbTxn* get_transaction();

		void close();

		int remove_environment();
		int remove_database();

		int truncate_database();
		int truncate_database(u_int32_t *countp);

		const char* getProgname();
		void setProgname(const char* progname);
	private:
		bool join_;

		DbEnv *dbenv;

		Db *db;

		std::string db_home_;
		std::string data_dir_;
		std::string log_dir_;
		std::string tmp_dir_;
		std::string dbFileName_;
		std::string progname_;

		std::ostream* error_stream;
		                                                                                                                                    
		// Make sure the default constructor is private
		// We don't want it used.
		FileCollection() : {}

		int environment_setup();
		void environment_teardown();
		int db_setup();
		void db_teardown();

		int make_dir(std::string& root_dir,std::string& sub_dir);
		int make_dir(std::string newdir);
		int make_dir(const char* newdir);

		DbEnv* getEnvironment();

		void print_exception_of_environment_setup(const char* what);
		void print_exception_of_environment_teardown(const char* what);
		void print_exception_of_db_setup(const char* what);
		void print_exception_of_db_teardown(const char* what);
		void print_exception_of_remove_database(const char* what);
		void print_exception_of_remove_environment(const char* what);
		void print_exception_of_get_environment(const char* what);

		u_int32_t get_environment_open_flag();
	}; 

}


#include "FileUtility.h"

using namespace std;

template <class KeyType,class DataType>
kaistizen::FileCollection<KeyType,DataType>::FileCollection(bool join,string &file)  
	:	join_(join),
		dbenv(NULL),				// Environment object
		db(NULL),					// Database object
		dbFileName_(file),			// Database file name
		error_stream(&std::cerr),
		progname_(DEFAULT_PROGRAM_NAME)
{
}

template <class KeyType,class DataType>
kaistizen::FileCollection<KeyType,DataType>::FileCollection(bool join,string &file,ostream* err_stream)
    :	join_(join),
		dbenv(NULL),				// Environment object
		db(NULL),					// Database object
		dbFileName_(file),			// Database file name
		error_stream(err_stream),
		progname_(DEFAULT_PROGRAM_NAME)
{	
}

template <class KeyType,class DataType>
kaistizen::FileCollection<KeyType,DataType>::FileCollection(bool join,string db_home,string data_dir,string log_dir,string tmp_dir,string &file,ostream* err_stream)
    :	join_(join),
		dbenv(NULL),				// Environment object
		db(NULL),					// Database object
		db_home_(db_home),
		data_dir_(data_dir),
		log_dir_(log_dir),
		tmp_dir_(tmp_dir),
		dbFileName_(file),			// Database file name
		error_stream(err_stream),
		progname_(DEFAULT_PROGRAM_NAME)
{
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::close()
{
    db_teardown();
	environment_teardown();
} 

// Note that any of the db calls can throw DbException
template <class KeyType,class DataType>
int
kaistizen::FileCollection<KeyType,DataType>::environment_setup()
{
	int ret = -1;

	try
	{
		if( (dbenv = getEnvironment()) == NULL ) {
			return -1;
		}

		// Open the environment with full transactional support.
		u_int32_t flag = get_environment_open_flag();

		if((ret = dbenv->open(db_home_.c_str(), flag, 0)) != 0) 
		{
			dbenv->err(ret, "DbEnv->open: data_dir(%s), log_dir(%s), tmp_dir(%s)",data_dir_,log_dir_,tmp_dir_);
			close();
			return (ret);
		}

		return (0);
	}
    catch(DbException &dbe)
    {
		print_exception_of_environment_setup(dbe.what());
    }
    catch(exception &e)
    {
		print_exception_of_environment_setup(e.what());
    }

	close();
	return (-1);
}

template <class KeyType,class DataType>
u_int32_t 
kaistizen::FileCollection<KeyType,DataType>::get_environment_open_flag()
{
	u_int32_t flag = 0;

	if(join_) {
		flag = environment_open_flags_for_join;
	} else {
		flag = environment_open_flags_for_init;
	}

	return flag;
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_environment_setup(const char* what)
{
	cerr << "Error setting up environment: data_dir(" << data_dir_ \
			<< "), log_dir(" << log_dir_ \
			<< "), tmp_dir(" << tmp_dir_ \
			<< ")" \
			<< endl;
	cerr << what << endl;
}

template <class KeyType,class DataType>
int
kaistizen::FileCollection<KeyType,DataType>::db_setup()
{
	int ret = -1;

	try
	{
		db = new Db(dbenv,0);
		db->set_error_stream(error_stream);

		if ((ret = db->open(NULL, dbFileName_.c_str(), NULL, DB_BTREE, database_open_flags, 0)) != 0) 
		{
			dbenv->err(ret, "DB->open: %s", dbFileName_.c_str());
			close();
			return (ret);
		}

		return (0);
	}
    catch(DbException &dbe)
    {
		print_exception_of_db_setup(dbe.what());
    }
    catch(exception &e)
    {
		print_exception_of_db_setup(e.what());
    }

	close();
	return (-1);
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_db_setup(const char* what)
{
	cerr << "Error setting up database: " << dbFileName_ << "\n";
	cerr << what << endl;
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::db_teardown()
{
	try
	{
		// Close the database handle before the environment handle.
		if (db != NULL)
		{
			(void)db->close(0);
			delete db;
			db = NULL;
		}
	} 
	catch(DbException &e)
    {
       print_exception_of_db_teardown(e.what());
    }
    catch(std::exception &e)
    {
       print_exception_of_db_teardown(e.what());
    }
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_db_teardown(const char* what)
{
	cerr << "Error tearing down database: " << dbFileName_ << "\n";
	cerr << what << endl;
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::environment_teardown()
{
	try
	{
		// Close the environment handle.
		if(dbenv != NULL)
		{
			(void)dbenv->close(0);
			delete dbenv;
			dbenv = NULL;
		}
	} 
	catch(DbException &e)
    {
		print_exception_of_environment_teardown(e.what());
    }
    catch(std::exception &e)
    {
		print_exception_of_environment_teardown(e.what());
    }
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_environment_teardown(const char* what)
{
	cerr << "Error tearing down environment: data_dir(" << data_dir_ \
			<< "), log_dir(" << log_dir_ \
			<< "), tmp_dir(" << tmp_dir_ \
			<< ")" \
			<< endl;
	cerr << what << endl;
}


template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::make_dir(string& root_dir,string& sub_dir)
{
	string total_path = root_dir + "\\" + sub_dir;

	return make_dir(total_path.c_str());
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::make_dir(string newdir)
{
	return make_dir(newdir.c_str());
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::make_dir(const char* newdir)
{
	return (int)FileUtility::make_dir(newdir);
}

 
template <class KeyType,class DataType>
int
kaistizen::FileCollection<KeyType,DataType>::open()
{
	int ret = -1;

	make_dir(db_home_);
	make_dir(db_home_,data_dir_);
	make_dir(db_home_,log_dir_);
	make_dir(db_home_,tmp_dir_);

	if((ret=environment_setup()) != 0) {
		return (ret);
	}

	if((ret=db_setup()) != 0) {
		return (ret);
	}

	return (0);
}


template <class KeyType,class DataType>
DbTxn * 
kaistizen::FileCollection<KeyType,DataType>::get_transaction()
{
	int ret = -1;
	DbTxn *tid = NULL;

	try
	{
		/* Begin the transaction. */
		if ((ret = dbenv->txn_begin(NULL, &tid, 0)) != 0) 
		{
			dbenv->err(ret, "DB_ENV->txn_begin");
			return (NULL);
		}
	}
	catch(DbException &e)
    {
		dbenv->errx("DbException beginning a transaction : %s",e.what());
    }
    catch(std::exception &e)
    {
		dbenv->errx("std::Exception beginning a transaction : %s",e.what());
    }

	if(ret != 0)
		return (NULL);
	else
		return tid;
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::put_entry(const KeyType *key,u_int32_t key_size,const DataType *data,u_int32_t data_size)
{
	return put_entry(key,key_size,data,data_size,NULL);
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::put_entry(const KeyType *key,u_int32_t key_size,const DataType *data,u_int32_t data_size,DbTxn *tid)
{
	int ret = -1;

	Dbt key_, data_;

	try
	{
		/* Initialization. */
		key_.set_data((void*)key);
		key_.set_size(key_size);
		data_.set_data((void*)data);
		data_.set_size(data_size);

		if(tid == NULL) {
			ret = db->put(NULL, &key_, &data_, DB_AUTO_COMMIT);
		} else {
			ret = db->put(tid, &key_, &data_, 0);
		}

		if(ret != 0 ) {
			dbenv->err(ret, "db->put: ");
			return (ret);
		}

		return (0);
	}
    catch(DbException &e)
    {
		dbenv->errx("DbException putting an entry : %s",e.what());
    }
    catch(std::exception &e)
    {
		dbenv->errx("std::Exception putting an entry : %s",e.what());
    }

	return (-1);
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::get_entry(const KeyType *key,u_int32_t key_size,DataType *data,u_int32_t data_length)
{
	return get_entry(key,key_size,data,data_length,NULL);
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::get_entry(const KeyType *key,u_int32_t key_size,DataType *data,u_int32_t data_length,DbTxn *tid)
{
	int ret = -1;

	Dbt key_, data_;

	try
	{
		/* Initialization. */
		key_.set_data((void*)key);
		key_.set_size(key_size);
		
		data_.set_data((void*)data);
		data_.set_ulen(data_length);
		data_.set_flags(DB_DBT_USERMEM);
		
		if((ret = db->get(tid, &key_, &data_, 0)) != 0 )
		{
			if( ret != DB_NOTFOUND) { 
				dbenv->err(ret, "db->get: ");
			}

			return (ret);
		}

		return (0);
	}
	catch(DbException &e)
    {
		dbenv->errx("DbException getting an entry : %s",e.what());
    }
    catch(std::exception &e)
    {
		dbenv->errx("std::Exception getting an entry : %s",e.what());
    }

	return (-1);
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::del_entry(const KeyType *key,u_int32_t key_size)
{
	return del_entry(key,key_size,NULL);
}
	
template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::del_entry(const KeyType *key,u_int32_t key_size,DbTxn *tid)
{
	int ret = -1;
	
	Dbt key_;

	try
	{
		/* Initialization. */
		key_.set_data((void*)key);
		key_.set_size(key_size);

		if(tid == NULL)	{
			ret = db->del(NULL, &key_, DB_AUTO_COMMIT);
		} else {
			ret = db->del(tid, &key_, 0);
		}
	}
	catch(DbException &e)
    {
		dbenv->errx("DbException getting an entry : %s",e.what());
    }
    catch(std::exception &e)
    {
		dbenv->errx("std::Exception getting an entry : %s",e.what());
    }

	return (ret);
}

template <class KeyType,class DataType>
DbEnv* 
kaistizen::FileCollection<KeyType,DataType>::getEnvironment()
{
	try
	{
		//
		// Create an environment object and initialize it for error
		// reporting.
		//
		DbEnv* environment = new DbEnv(0);
		environment->set_error_stream(error_stream);
		environment->set_errpfx(progname_.c_str());
		
		//
		// We want to specify the shared memory buffer pool cachesize,
		// but everything else is the default.
		//
		environment->set_cachesize(0, 64 * 1024, 0);

		// environment->set_timeout(10*1000,DB_SET_LOCK_TIMEOUT);
		// environment->set_timeout(10*1000,DB_SET_TXN_TIMEOUT);

		(void)environment->set_data_dir(data_dir_.c_str());
		(void)environment->set_lg_dir(log_dir_.c_str());
		(void)environment->set_tmp_dir(tmp_dir_.c_str());

		return environment;
	}
	catch(DbException &e)
    {
		print_exception_of_get_environment(e.what());
    }
    catch(std::exception &e)
    {
		print_exception_of_get_environment(e.what());
    }

	return (NULL);
}


template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_get_environment(const char* what)
{
	cerr << "Error removing the environment: data_dir(" << data_dir_ \
			<< "), log_dir(" << log_dir_ \
			<< "), tmp_dir(" << tmp_dir_ \
			<< ")" \
			<< endl;
	cerr << what << endl;
}

template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::remove_database()
{
	int ret = -1;
	
	try
	{
		DbEnv* environment = getEnvironment();
		
		u_int32_t flag = get_environment_open_flag();

		// Open the environment with full transactional support.
		if((ret = environment->open(db_home_.c_str(), flag, 0)) != 0) 
		{
			environment->err(ret, "DbEnv->open: data_dir(%s), log_dir(%s), tmp_dir(%s)",data_dir_,log_dir_,tmp_dir_);
			close();
			return (ret);
		}

		environment->dbremove(NULL,dbFileName_.c_str(),NULL,DB_AUTO_COMMIT);
		environment->close(0);
		delete environment;

        return (0);
	}
	catch(DbException &e)
    {
		print_exception_of_remove_database(e.what());	
    }
    catch(std::exception &e)
    {
		print_exception_of_remove_database(e.what());	
    }

	return (-1);
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_remove_database(const char* what)
{
	cerr << "Error removing the database: name(" << dbFileName_ \
			<< ")" \
			<< endl;
	cerr << what << endl;
}

// Remove the shared database regions.
template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::remove_environment()
{
	int ret = -1;
	
	try
	{
		DbEnv* environment = getEnvironment();
		
		ret = environment->remove(db_home_.c_str(), 0);

		delete environment;

        return ret;
	}
	catch(DbException &e)
    {
		print_exception_of_remove_environment(e.what());
    }
    catch(std::exception &e)
    {
        print_exception_of_remove_environment(e.what());
    }

	return (ret);
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::print_exception_of_remove_environment(const char* what)
{
	cerr << "Error removing the environment: data_dir(" << data_dir_ \
			<< "), log_dir(" << log_dir_ \
			<< "), tmp_dir(" << tmp_dir_ \
			<< ")" \
			<< endl;
	cerr << what << endl;
}

template <class KeyType,class DataType>
const char*
kaistizen::FileCollection<KeyType,DataType>::getProgname()
{
	return progname_.c_str();
}

template <class KeyType,class DataType>
void
kaistizen::FileCollection<KeyType,DataType>::setProgname(const char* progname)
{
	progname_ = progname;
}


template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::truncate_database()
{
	return truncate_database(NULL);
}

// Remove the shared database regions.
template <class KeyType,class DataType>
int 
kaistizen::FileCollection<KeyType,DataType>::truncate_database(u_int32_t *countp)
{
	int ret = -1;
	
	try
	{
		if(countp == NULL) {
			ret = db->truncate(NULL,NULL,DB_AUTO_COMMIT);
		} else {
			ret = db->truncate(NULL,countp,DB_AUTO_COMMIT);
		}

		if( ret != 0 )
		{
			dbenv->err(ret,"db->truncate : %s",dbFileName_.c_str());
			return (-1);
		}

		return (0);
	}
	catch(DbException &e)
    {
		dbenv->errx("DbException trucating a database : %s",e.what());
    }
    catch(std::exception &e)
    {
		dbenv->errx("std:exception trucating a database : %s",e.what());
    }

	return (-1);
}

#endif