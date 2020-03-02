;created using example @ http://codebetter.com/petervanooijen/2006/08/03/deploying-mobile-apps-the-easy-way-using-inno-setup/
; #defines require the ISPP add-on: http://sourceforge.net/projects/ispp/
#define APP "FSCruiserV2"

#define APP_VERSION "2020.03.02"
#define SPECIALTAG "pre"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define ORGANIZATION "U.S. Forest Service, Forest Management Service Center"

#define FSCRUISER_INI "FSCruiserV2.ini"

[Setup]
AppName         =FSCruiser V2
AppVersion      ={#APP_VERSION}
AppVerName      =FSCruiser version {#APP_VERSION} for Windows CE, and Windows Mobile devices
AppPublisher    ={#ORGANIZATION}
AppPublisherURL ={#BASEURL}
AppSupportURL   ={#BASEURL}/support.shtml
AppUpdatesURL   ={#BASEURL}/cruising/index.shtml

LicenseFile=..\LICENSE.md

;this is not a tyical installer and doesn't install anything on the users computer
Uninstallable   = no 
CreateAppDir    = no


DisableProgramGroupPage = yes
DefaultGroupName        =FMSC\{#APP}


OutputBaseFilename=FScruiserV2_FDR
Compression=lzma
SolidCompression=yes
PrivilegesRequired=lowest


InfoBeforeFile=..\Documentation\ConnectingToDevice.md

[Files]
;FScruiser mobile files
Source: {#FSCRUISER_INI}; DestDir: "{localappdata}\{#APP}\FDR_Install"; Flags: ignoreversion; 
Source: "..\Source\FSCruiserV2_CF35_CAB\Release\FSCruiserV2.CAB"; DestDir: "{localappdata}\{#APP}\FDR_Install"; Flags: ignoreversion; 

;Documentation
Source: "..\Documentation\FScruiserV2UserGuide.docx"; DestName:"FScruiserV2UserGuide.docx"; DestDir: "{userappdata}\{#APP}"; Flags: ignoreversion;

[Icons]
Name: {group}\FScruiser V2 User Guide.docx; Filename: {localappdata}\{#APP}\FScruiser V2 User Guide.docx

[Code]
function GetCEappManager(Param : string) : string;
var Path: String;
begin
  Path:= '';
  RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CEAPPMGR.EXE','', Path)
    if Path = '' then
      if FileExists('C:\Windows\WindowsMobile\ceappmgr.exe') then
        Path:= 'C:\Windows\WindowsMobile\ceappmgr.exe';

  if Path = '' then
  begin
    MsgBox('Unable to locate CEAppMgr.exe Ensure that Windows Mobile Device Center or ActiveSync is installed', mbInformation, MB_OK);
  end;
  result:=  Path;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var 
CEAppMgrPath : String;
ErrorCode : Integer;
Prams : String;
begin
  if ((CurStep=ssPostInstall)) then
  begin
    CEAppMgrPath:= GetCEappManager('');
    if CEAppMgrPath <>'' then
    begin
      Prams:= ExpandConstant(' "{localappdata}\{#APP}\FDR_Install\{#FSCRUISER_INI}"');

      ExecAsOriginalUser(CEAppMgrPath , Prams, '', SW_SHOW, ewNoWait, ErrorCode);  
    end;
  end;   
end;