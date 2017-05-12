@ECHO OFF
SETLOCAL ENABLEEXTENSIONS

::Boilderplate 
::detect if invoked via Window Explorer
SET interactive=1
ECHO %CMDCMDLINE% | FIND /I "/c" >NUL 2>&1
IF %ERRORLEVEL% == 0 SET interactive=0

pandoc test.md -o pdf\test.pdf

IF "%interactive%"=="0" PAUSE
ENDLOCAL
EXIT /B 0