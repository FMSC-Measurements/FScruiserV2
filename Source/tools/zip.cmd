SET sevenZip=%programfiles%\7-Zip\7z.exe

if exist "%sevenZip%" (
  "%sevenZip%" %*
)