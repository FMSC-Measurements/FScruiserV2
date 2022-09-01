
#define MsBuildOutputDir "..\Source\FSCruiserV2\bin\Release\net462"
#define APP "FSCruiserV2"

#define APP_VERSION "2022.08.31"
#define SPECIALTAG ""
#define BASEURL "https://www.fs.usda.gov/forestmanagement/products/measurement"
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

DefaultDirName={autopf}\FMSC\{#APP}
DefaultGroupName=FMSC\{#APP}
UsePreviousAppDir=no


;specifies the file version on the setup exe
VersionInfoVersion={#APP_VERSION}

CreateAppDir=yes
OutputBaseFilename=FScruiserV2_PC

Compression=lzma
SolidCompression=yes

PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog

;dont allow program to be installed on network drives
AllowUNCPath=no
AllowNetworkDrive=no

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons};

[Files]
Source: "{#MsBuildOutputDir}\FScruiserPC.exe"; DestDir: {app}; Flags: ignoreversion;
Source: "{#MsBuildOutputDir}\*.dll"; DestDir: {app}; Flags: ignoreversion;
Source: "{#MsBuildOutputDir}\runtimes\win-x64\native\*.dll"; DestDir: {app}\runtimes\win-x64\native; Flags: ignoreversion;
Source: "{#MsBuildOutputDir}\runtimes\win-x86\native\*.dll"; DestDir: {app}\runtimes\win-x86\native; Flags: ignoreversion;
Source: "{#MsBuildOutputDir}\Sounds\*";  DestDir: {app}\Sounds; Flags: ignoreversion;
Source: "{#MsBuildOutputDir}\FScruiserPC.exe.config"; DestDir: {app}; Flags: ignoreversion;
                                                     

[Icons]
Name: {autodesktop}\FScruiserV2; Filename: {app}\FScruiserPC.exe; Tasks: desktopicon
Name: {group}\FScruiserV2; Filename: {app}\FScruiserPC.exe;




