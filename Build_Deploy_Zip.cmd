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
SET zip="%parent%Source\tools\zip.cmd"

IF NOT DEFINED verStamp (SET verStamp=%date:~10,4%%date:~4,2%%date:~7,2%)

set outFile=%parent%Deployment\Output\FScruiser_%verStamp%.zip

cd .\Source\FSCruiserV2\bin\Release\net462

call %zip% a -tzip -spf %outFile%  FScruiserPC.exe FScruiserPC.exe.config *.dll runtimes\win-x64\native\*.dll runtimes\win-x86\native\*.dll Sounds\*

::if invoked from windows explorer, pause
IF "%interactive%"=="0" PAUSE
ENDLOCAL
EXIT /B 0