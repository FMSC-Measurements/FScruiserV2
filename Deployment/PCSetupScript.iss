#define APP "FSCruiserV2"

#define APP_VERSION "2017.02.13"
#define SETUPVERSION "20170213"
#define SPECIALTAG "Production"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define ORGANIZATION "U.S. Forest Service, Forest Management Service Center"


[Setup]
AppName=FSCruiser V2
AppVersion={#APP_VERSION}
AppVerName=FSCruiser version {#SETUPVERSION} for Windows Desktop
AppPublisher={#ORGANIZATION}
AppPublisherURL={#BASEURL}
AppSupportURL={#BASEURL}/support.shtml
AppUpdatesURL={#BASEURL}/cruising/index.shtml

DefaultDirName={pf32}\FMSC\{#APP}
DefaultGroupName=FMSC\{#APP}
UsePreviousAppDir=no

CreateAppDir=yes
OutputBaseFilename=FScruiserV2_PC_{#SETUPVERSION}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
ShowLanguageDialog=no

[Files]
Source: "..\Source\FSCruiserV2\bin\PC\Release\FScruiserPC.exe"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Source\FSCruiserV2\bin\PC\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Source\FSCruiserV2\bin\PC\Release\x64\*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Source\FSCruiserV2\bin\PC\Release\x86\*.dll"; DestDir: "{app}"; Flags: ignoreversion;


[Icons]
Name: {userdesktop}\FScruiserV2; Filename: {app}\FScruiserPC.exe;
Name: {group}\FScruiserV2; Filename: {app}\FScruiserPC.exe;




