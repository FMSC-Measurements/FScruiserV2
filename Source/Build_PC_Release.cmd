@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

::Boilderplate 

::name of this script
SET me=%~n0
::directory of script
SET parent=%~dp0

ECHO %me%

SET msbuildPath="C:\Program Files (x86)\MSBuild\14.0\bin\amd64\MSBuild.exe"

%msbuildPath%  %parent%\FSCruiserV2\FScruiserPC.csproj /target:Rebuild /property:Configuration=Release;Platform=AnyCPU;SolutionDir=%parent%\

EXIT /B %errorlevel%