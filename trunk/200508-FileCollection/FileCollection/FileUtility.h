#ifndef FILE_UTILITY_H__
#define FILE_UTILITY_H__

namespace kaistizen
{
	class FileUtility
	{
	public:
		static int del_dir(const char* root_dir, bool bDeleteSubdirectories); 
		static int make_dir(const char* newdir);
	private:
		FileUtility() {};

		static int make_dir_sub(const char* dirname);  

	};
}

#endif 
