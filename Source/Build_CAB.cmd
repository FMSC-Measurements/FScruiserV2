@ECHO OFF
SETLOCAL ENABLEEXTENSIONS
:::::::::::::::::
::Boilderplate 

::name of this script
SET me=%~n0

ECHO %me%

::directory of script
SET parent=%~dp0

:::::::::::::::::
::variables
SET devenvPath=C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe
SET buildLogPath=%parent%\pack.log
SET cabOutDir=%parent%\FSCruiserV2_CF35_CAB\Release\

IF NOT DEFINED verStamp (SET verStamp=%date:~10,4%%date:~4,2%%date:~7,2%)

::if build log exists delete because devenv only appends to our log file
IF EXIST "%buildLogPath%" DEL /Q "%buildLogPath%"

::delete any existing cab file, to ensure we arnt working with a stale cab file
IF EXIST "%cabOutDir%FScruiserV2.CAB" DEL /Q "%cabOutDir%FScruiserV2.CAB"

ECHO %me%:Start Building Cab
"%devenvPath%" %parent%\FScruiserV2.VS08.sln /build "Release|AnyCPU" /project "FSCruiserV2_CF35_CAB" /Out "%buildLogPath%"

IF /I "%ERRORLEVEL%" NEQ "0" (
    ::display build log
    TYPE %buildLogPath%
    ECHO %me%:Build Failed
    EXIT /B 1
)

::display build log
TYPE %buildLogPath%

COPY /Y /B "%cabOutDir%FScruiserV2.CAB" "%cabOutDir%FScruiserV2_%verStamp%.CAB"

EXIT /B %ERRORLEVEL%

