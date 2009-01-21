#include "stdafx.h"
#include "FileUtility.h"

#include <sys/stat.h>
#include <direct.h>

#include <string>
#include <iostream>
#include <windows.h>
#include <conio.h>

int 
kaistizen::FileUtility::del_dir(const char* root_dir, bool bDeleteSubdirectories = true)
{
	std::string refcstrRootDirectory = root_dir;
	bool            bSubdirectory = false;       // Flag, indicating whether
												// subdirectories have been found
	HANDLE          hFile;                       // Handle to directory
	std::string     strFilePath;                 // Filepath
	std::string     strPattern;                  // Pattern
	WIN32_FIND_DATA FileInformation;             // File information


	strPattern = refcstrRootDirectory + "\\*.*";
	hFile = ::FindFirstFile(strPattern.c_str(), &FileInformation);
	if(hFile != INVALID_HANDLE_VALUE)
	{
		do
		{
			if(FileInformation.cFileName[0] != '.')
			{
				strFilePath.erase();
				strFilePath = refcstrRootDirectory + "\\" + FileInformation.cFileName;

				if(FileInformation.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
				{
				if(bDeleteSubdirectories)
				{
					// Delete subdirectory
					int iRC = del_dir(strFilePath.c_str(), bDeleteSubdirectories);
					if(iRC)
					return iRC;
				}
				else
					bSubdirectory = true;
				}
				else
				{
					// Set file attributes
					if(::SetFileAttributes(strFilePath.c_str(),
											FILE_ATTRIBUTE_NORMAL) == FALSE)
						return ::GetLastError();

					// Delete file
					if(::DeleteFile(strFilePath.c_str()) == FALSE)
						return ::GetLastError();
				}
			}
		} while(::FindNextFile(hFile, &FileInformation) == TRUE);

		// Close handle
		::FindClose(hFile);

		DWORD dwError = ::GetLastError();
		if(dwError != ERROR_NO_MORE_FILES)
			return dwError;
		else
		{
			if(!bSubdirectory)
			{
				// Set directory attributes
				if(::SetFileAttributes(refcstrRootDirectory.c_str(),
									FILE_ATTRIBUTE_NORMAL) == FALSE)
				return ::GetLastError();

				// Delete directory
				if(::RemoveDirectory(refcstrRootDirectory.c_str()) == FALSE)
					return ::GetLastError();
			}
		}
	}

	return 0;
}

int 
kaistizen::FileUtility::make_dir_sub(const char* dirname)
{
    int ret=0;
#ifdef WIN32
    ret = mkdir(dirname);
#else
	#ifdef unix
		ret = mkdir(dirname,0775);
	#else
		#ifdef __TURBOC__
			ret = mkdir (dirname);
		#endif
	#endif
#endif

    return ret;
}

int 
kaistizen::FileUtility::make_dir(const char* newdir)
{
	char *buffer = NULL;
	char *p = NULL;
	size_t len = strlen(newdir);  

	if (len <= 0) 
		return 0;

	buffer = (char*)malloc(len+1);
	strcpy(buffer,newdir);

	if (buffer[len-1] == '/') 
		buffer[len-1] = '\0';

	if (make_dir_sub(buffer) == 0) {
		free(buffer);
		return 1; 
	}
	
	p = buffer+1;

	for(;;) {
		char hold;

		while(*p && *p != '\\' && *p != '/')
			p++;
		hold = *p;

		*p = 0;
		if ((make_dir_sub(buffer) == -1) && (errno == ENOENT)) {
			printf("couldn't create directory %s\n",buffer);
			free(buffer);
			return 0;
		}  // if

		if (hold == 0)
			break;

		*p++ = hold;
	} // while

	free(buffer);
	return 1;
} // mkdir()

