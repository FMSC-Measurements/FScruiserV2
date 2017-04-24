@ECHO OFF
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

::variables
IF NOT DEFINED verStamp (SET verStamp=%date:~10,4%%date:~4,2%%date:~7,2%)

::Build CAB File. This generates FScruiser.CAB and FScruiser_########.cab 
CALL ./Source/Build_CAB

IF /I "%errorlevel%" NEQ "0" (
IF "%interactive%"=="0" PAUSE
EXIT /B 1
)

::Build Inno Setup Installer
SET appVer=%date:~10,4%.%date:~4,2%.%date:~7,2%

"C:\Program Files (x86)\Inno Setup 5\iscc" /dAPP_VERSION=%appVer% /F"FScruiserV2_FDR_%verStamp%" "./Deployment/CFSetupScript.iss"  

IF "%interactive%"=="0" PAUSE
ENDLOCAL
EXIT /B 0