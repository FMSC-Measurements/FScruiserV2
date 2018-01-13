::Boilderplate 
@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

::name of this script
SET me=%~n0
::directory of script
SET parent=%~dp0

ECHO %me%
::end Boilderplate

SET vsWherePath="C:\Program Files (x86)\Microsoft Visual Studio\Installer\vswhere.exe"

for /f "usebackq tokens=1* delims=: " %%i in (`%vsWherePath% -latest -requires Microsoft.Component.MSBuild`) do (
  if /i "%%i"=="installationPath" set InstallDir=%%j
)

if exist "%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" (
  SET msbuildPath="%InstallDir%\MSBuild\15.0\Bin\MSBuild.exe" %*
)

%msbuildPath%  %parent%\FSCruiserV2\FScruiserPC.csproj /target:Rebuild /property:Configuration=Release;Platform=AnyCPU;SolutionDir=%parent%\

EXIT /B %errorlevel%