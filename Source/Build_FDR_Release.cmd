@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

::name of this script
SET me=%~n0

ECHO %me%

::directory of script
SET parent=%~dp0

SET msbuildPath=C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe

%msbuildPath%  %parent%\FSCruiserV2\FScruiserV2CE_CF20.csproj /target:Rebuild /property:Configuration=Release;Platform=AnyCPU;SolutionDir=%parent%

EXIT /B %errorlevel%