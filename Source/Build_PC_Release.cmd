@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

::Boilderplate 

::name of this script
SET me=%~n0
::directory of script
SET parent=%~dp0

ECHO %me%

SET msbuildPath=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe

%msbuildPath%  %parent%\FSCruiserV2\FScruiserPC.csproj /target:Rebuild /property:Configuration=Release;Platform=AnyCPU;SolutionDir=%parent%\

EXIT /B 0