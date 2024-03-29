::@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

::Boilderplate 
::detect if invoked via Window Explorer
SET interactive=1
ECHO %CMDCMDLINE% | FIND /I "/c" >NUL 2>&1
IF %ERRORLEVEL% == 0 SET interactive=0

::name of this script
SET me=%~n0
::directory of script
SET parent=%~dp0


IF NOT DEFINED verStamp (SET verStamp=%date:~10,4%%date:~4,2%%date:~7,2%)

CALL %parent%/Source/Build_PC_Release.cmd

IF /I "%ERRORLEVEL%" NEQ "0" (
ECHO build failed
IF "%interactive%"=="0" PAUSE
EXIT /B 1
)

SET innoSetupPath="%localappdata%\Programs\Inno Setup 6\ISCC.exe"
IF NOT EXIST %innoSetupPath% (
	SET innoSetupPath="%ProgramFiles(x86)%\Inno Setup 6\ISCC.exe")

IF NOT EXIST %innoSetupPath% (
	ECHO "inno setup not found"
	IF "%interactive%"=="0" PAUSE
	EXIT /B 1)

%innoSetupPath% /dAPP_VERSION=%appVer% /F"FScruiserV2_PC_%verStamp%" "./Deployment/PCSetupScript.iss" 

::if invoked from windows explorer, pause
IF "%interactive%"=="0" PAUSE
ENDLOCAL
EXIT /B 0
