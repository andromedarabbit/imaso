@echo off
SETLOCAL
call "%VS90COMNTOOLS%\..\..\VC\vcvarsall.bat" x86
"%WINDIR%\Microsoft.NET\Framework\v3.5\msbuild.exe" %*
SET ERR_LEVEL=%errorlevel%
ENDLOCAL
exit /b %ERR_LEVEL%
