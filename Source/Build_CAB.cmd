@ECHO OFF

 
SET devenvPath=C:\Program Files (x86)\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe
SET verStamp=%date:~10,4%%date:~4,2%%date:~7,2%
SET buildLogPath=%cd%\pack.log
SET cabOutDir=.\FSCruiserV2CECF20_CAB\Release\

::if build log exists delete because devenv only appends to our log file
IF EXIST "%buildLogPath%" DEL /Q "%buildLogPath%"

::delete any existing cab file, to ensure we arnt working with a stale cab file
IF EXIST "%cabOutDir%FScruiserV2.CAB" DEL /Q "%cabOutDir%FScruiserV2.CAB"

"%devenvPath%" .\FScruiserV2.sln /build "Release|AnyCPU" /project "FSCruiserV2CECF20_CAB" /Out "%buildLogPath%"

::display build log
TYPE %buildLogPath%

ECHO COPY /Y /B "%cabOutDir%FScruiserV2.CAB" "%cabOutDir%FScruiserV2_%verStamp%.CAB"

::
EXIT %ERRORLEVEL%