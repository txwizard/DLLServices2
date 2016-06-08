@echo off
goto SKIPREM

::  ==========================================================================
::
::  Name:               DocumentTestStandRun.CMD
::
::  Synopsis:           Exercise DLLServices2TestStand.exe, and document the
::                      outcome.
::
::  Arguments:          This shell script has no arguments.
::
::  Dependencies:       DLLServices2TestStand.exe   = This is the program to run
::                                                    and document the outcome.
::
::                      Date2FN.exe                 = This 32-bit character mode
::                                                    Windows program appends a
::                                                    formatted string represen-
::                                                    ting the LastWriteTime of
::                                                    a file to its base name to
::                                                    create a name that is
::                                                    intended to be locally
::                                                    unique. The optional R
::                                                    switch instructs it to
::                                                    mark the file as read only
::                                                    by setting the read only
::                                                    attribute in the file
::                                                    system.
::
::                      ExportFromEventLogNDays.CMD = This shell script uses the
::                                                    SysInternals psloglist.exe
::                                                    utility to extract records
::                                                    from a Windows Event Log.
::                                                    The extracted records go
::                                                    into a tab delimited ASCII
::                                                    file that I import into a
::                                                    Microsoft Excel workbook.
::
::                      ShowTime.CMD                = This little shell script
::                                                    displays the current time
::                                                    per the machine clock.
::
::                      WWPause.exe                 = This 32-bit character mode
::                                                    Windows program replaces
::                                                    the lame intrinsic pause
::                                                    command implemented by the
::                                                    command processor. Unlike
::                                                    Pause, it insists on the
::                                                    RETURN key being pressed.
::
::                      DLLServices2TestStand.exe must be installed into the
::                      following directory.
::
::                          C:\Users\DAVE\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\DLLServices2TestStand\bin\Debug
::
::                      All other dependent programs must be installed into a
::                      directory that is in the Windows PATH list.
::
::  Remarks:            Companion script CaptureTestStandOutput.CMD captures its
::                      standard output into a text file, to which it eventually
::                      assigns a unique name.
::
::                      This script has almost everything it needs to be a
::                      general purpose test platform.
::
::  ----------------------------------------------------------------------------
::  Revision History
::  --------------------------------------------------------------------------
::
::  Date       Author Version Synopsis
::  ---------- ------ ------- --------------------------------------------------
::  2015/06/19 DAG/WW 1.00    This script was created by carefully trimming from
::                            its twin, CaptureTestStandOutput.CMD.
::  2015/06/23 DAG/WW 1.00    Correct a minor formatting error, more annoyance
::                            than error, in the flower box.
::  ============================================================================

:SKIPREM

    echo.
    echo BOJ %0, version 1.00
    echo.
    call ShowTime.CMD
    echo.
    echo ---------------------------------------------------------------------
    echo This script is about to launch DLLServices2TestStand.exe. At present,
    echo there are two silent prompts, both of which require you to press the
    echo ENTER key. Press ENTER now to launch DLLServices2TestStand.exe.
    echo ---------------------------------------------------------------------
    echo.
    WWPause.exe
    call ShowTime.CMD
    echo.
    "C:\Users\DAVE\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\DLLServices2TestStand\bin\Debug\DLLServices2TestStand.exe"
    echo DLLServices2TestStand.exe exit code = %ERRORLEVEL%

    if exist "C:\Users\DAVE\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\_NOTES\DLLServices2TestStand_5.5_Trace.LOG" (
        Date2FN.exe -r "C:\Users\DAVE\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\_NOTES\DLLServices2TestStand_5.5_Trace.LOG"
    ) else (
        echo.
        echo ---------------------------------------------------------------------
        echo C:\Users\DAVE\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\_NOTES\DLLServices2TestStand_5.5_Trace.LOG does not exist.
        echo ---------------------------------------------------------------------
        echo.
    )

    call ExportFromEventLogNDays.CMD Application "%USERPROFILE%\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\_NOTES\ApplicationEvents_%COMPUTERNAME%.TXT"
    Date2fn.exe -r "%USERPROFILE%\Documents\Visual Studio 2013\Projects\WizardWrx_Libs\DLLServices2\_NOTES\ApplicationEvents_%COMPUTERNAME%.TXT"
    echo.
    echo EOJ %~nx0
    echo.
    WWPause.exe