@echo off
echo %0 Begin

:: 2012/08/25 DAG Replace hard coded paths with environment variables.
:: 2014/06/07 DAG Adapt to meed the needs of a new solution.

echo.
echo -------------------------------------------------------------------------
echo Project Assembly = %USERPROFILE%\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\DLLServices2\DLLServices2\bin\Release\WizardWrx.DLLServices2.dll
echo Backup Directory = C:\COMMON_DATA\Assorted_Zips\program_source\Visual Studio 2010
echo Backup Password  = Default
echo -------------------------------------------------------------------------
echo.

"%USERPROFILE%\My Documents\Visual Studio 2010\Projects\Tools_for_Programmers\WWSourceCodeArchiver\WWSourceCodeArchiver\bin\Release\WWSourceCodeArchiver.exe" "-p%USERPROFILE%\My Documents\Visual Studio 2010\Projects\WizardWrx_Libs\DLLServices2\DLLServices2\bin\Release\WizardWrx.DLLServices2.dll" "-tC:\COMMON_DATA\Assorted_Zips\program_source\Visual Studio 2010"

echo %0 Done!
wwpause.exe