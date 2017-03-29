#define APP "FSCruiserV2"

#define APP_VERSION "2017.03.29"
#define SPECIALTAG "Production"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define ORGANIZATION "U.S. Forest Service, Forest Management Service Center"


[Setup]
AppName=FSCruiser V2
AppId=FScruiserPC
AppMutex=FScruiser   
AppVersion={#APP_VERSION}
AppVerName=FSCruiser version {#APP_VERSION} for Windows Desktop

LicenseFile=..\LICENSE.md

AppPublisher={#ORGANIZATION}
AppPublisherURL={#BASEURL}
AppSupportURL={#BASEURL}/support.shtml
AppUpdatesURL={#BASEURL}/cruising/index.shtml

DefaultDirName={pf32}\FMSC\{#APP}
DefaultGroupName=FMSC\{#APP}
UsePreviousAppDir=no


;specifies the file version on the setup exe
VersionInfoVersion={#APP_VERSION}

CreateAppDir=yes
OutputBaseFilename=FScruiserV2_PC

Compression=lzma
SolidCompression=yes

PrivilegesRequired=admin

;dont allow program to be installed on network drives
AllowUNCPath=no
AllowNetworkDrive=no

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons};

[Files]
Source: "..\Source\FSCruiserV2\bin\PC\Release\FScruiserPC.exe"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Source\FSCruiserV2\bin\PC\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Source\FSCruiserV2\bin\PC\Release\x64\*.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\Source\FSCruiserV2\bin\PC\Release\x86\*.dll"; DestDir: "{app}"; Flags: ignoreversion;


[Icons]
Name: {userdesktop}\FScruiserV2; Filename: {app}\FScruiserPC.exe; Tasks: desktopicon
Name: {group}\FScruiserV2; Filename: {app}\FScruiserPC.exe;




